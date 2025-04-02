using Crolow.FastDico.Dawg;
using Crolow.FastDico.Dicos;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.ScrabbleApi.Utils;
using Crolow.FastDico.Search;
using Crolow.FastDico.Utils;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Crolow.FastDico.ScrabbleApi;

/// <summary>
/// It's just a starting point
/// Still need implementation
/// Thinking of using gaddag and dawg together :
/// - Gaddag to search from RTL and LTR at the first pivot position
/// - Dawg to search from the first pivot position only LTR
/// </summary>
public partial class ScrabbleAI
{
    private GadDagCompiler dico;
    private PlayConfiguration playConfiguration;
    private Board board;
    private GameConfig gameConfig;
    private LetterBag letterBag;
    private DawgSearch gaddag;
    private PlayerRack rack;
    private CurrentGame currentGame;
    private GameConfig currentGameConfig;
    private PivotBuilder pivotBuilder;

    public ScrabbleAI(string configsFile, string configName)
    {
        playConfiguration = new ConfigReader().ReadConfiguration(configsFile, configName);
        this.currentGame = new CurrentGame();
        this.currentGame.Configuration = playConfiguration;

        currentGameConfig = playConfiguration.SelectedConfig;
        GadDagCompiler gaddag = new GadDagCompiler();
        gaddag.ReadFromFile(playConfiguration.SelectedConfig.GaddagFile);

        this.board = new Board(this.currentGame);
        this.letterBag = new LetterBag(this.currentGame);
        this.rack = new PlayerRack();
        this.dico = gaddag;
        this.gameConfig = playConfiguration.SelectedConfig;
        this.pivotBuilder = new PivotBuilder(board, gaddag.Root, playConfiguration);

        this.currentGame.LetterBag = this.letterBag;
        this.currentGame.Rack = this.rack;
    }

    public void StartGame()
    {
        using (StopWatcher stopwatch = new StopWatcher("Game started"))
        {
            NextRound(true);
        }
    }
    private void NextRound(bool firstMove)
    {
        var letters = letterBag.DrawLetters(rack);

        // Test pivots
        //for (int i = 6; i <= 8; i += 2)
        //{
        //    var test = letterBag.DrawLetters(rack);
        //    var j = 5;
        //    foreach (var t in test)
        //    {
        //        board.SetTile(0, i, j++, t);
        //    }
        //}
        //board.TransposeGrid();
        //pivotBuilder.Build();


        // End Test
        if (letters == null)
        {
            EndGame();
            return;
        }

        string res = TilesUtils.ConvertBytesToWord(letters);
        Console.WriteLine($"Rack :#{currentGame.Round}  {res}");


        var playedRounds = new PlayedRounds(gameConfig);

        // We set the original position to place which is at the board center
        if (firstMove)
        {
            int grid = 0;
            Position p = new Position((board.CurrentBoard[0].SizeH - 1) / 2, (board.CurrentBoard[0].SizeV - 1) / 2, grid);
            playedRounds.CurrentRound = new PlayedRound();
            playedRounds.CurrentRound.Position = new Position(p);
            board.TransposeGrid();
            SearchNodes(grid, dico.Root, 1, p, letters, playedRounds, p, true);
        }
        else
        {
            // Once we get the rack 
            // We create a grid with all possibilities at each pivot place
            pivotBuilder.Build();

            // Here begins the story... 
            // Related to the DAWG search we are only starting on squares that can
            // be connected 

            // Horizontal Search
            Search(0, letters, playedRounds);
            // Vertical Search
            Search(1, letters, playedRounds);
        }

        var selectedRound = playedRounds.Rounds.FirstOrDefault();
        if (selectedRound == null)
        {
            EndGame(); return;
        }

        // We remove letters played from the rack
        foreach (var letter in selectedRound.Tiles)
        {
            if (letter.Parent.Status != 1)
            {
                rack.RemoveTile(letter);
            }
        }

        selectedRound.FinalizeRound();
        board.SetRound(selectedRound);
        currentGame.RoundsPlayed.Add(selectedRound);
        selectedRound = new PlayedRound();
        currentGame.Round++;
#if DEBUG
        //  PrintGrid();
        //Console.ReadLine();
#endif
        NextRound(false);
    }

    private void Search(int grid, List<Tile> letters, PlayedRounds playedRounds)
    {
        for (var i = 1; i < board.CurrentBoard[grid].SizeV - 1; i++)
        {
            var rightToLeft = true;
            int oldj = 1;
            for (var j = 1; j < board.CurrentBoard[grid].SizeH - 1; j++)
            {
                var currentNode = dico.Root;
                playedRounds.CurrentRound = new PlayedRound();

                var sq = board.GetSquare(grid, j, i);
                if (sq.Status == -1)
                {
                    if (CheckConnect(grid, j, i))
                    {
                        Position start = new Position(j, i, grid);
                        Position firstPosition = new Position(j, i, grid);
                        // If we are skipping only one square we do not need
                        // to search on the left.
                        rightToLeft = j == 1 || (j == oldj + 1 ? true : false);

                        // If there is a filled squared on the left
                        // We need to prefill the current solution
                        var sqLeft = board.GetSquare(grid, j - 1, i);
                        if (sqLeft.Status == 1)
                        {
                            rightToLeft = false;
                            var sql = new List<Square>();
                            sql.Add(sqLeft);
                            var pos = j - 2;
                            while (true)
                            {
                                var sqNext = board.GetSquare(grid, pos, i);
                                if (sqNext.Status == 1)
                                {
                                    sql.Add(sqNext);
                                    pos--;
                                }
                                else
                                {
                                    sql.Reverse();
                                    playedRounds.CurrentRound = new PlayedRound();
                                    firstPosition = new Position(pos + 1, firstPosition.Y, grid);
                                    playedRounds.CurrentRound.Position = firstPosition;
                                    int x = firstPosition.X;
                                    int y = firstPosition.Y;

                                    foreach (var item in sql)
                                    {
                                        var parent = board.GetSquare(grid, x++, y);
                                        playedRounds.CurrentRound.AddTile(item.CurrentLetter, parent);
                                    }
                                    break;
                                }
                            }

                            foreach (var letter in sql)
                            {
                                try
                                {
                                    currentNode = currentNode.Children.First(p => p.Letter == letter.CurrentLetter.Letter);
                                }
                                catch (Exception ex)
                                {
                                    TilesUtils.ConvertBytesToWord(sql.Select(p => p.CurrentLetter.Letter).ToList());
                                }
                            }

                            // Ok we can process that square only if there are children
                            if (currentNode.Children.Any())
                            {
                                SearchNodes(grid, currentNode, 1, start, letters, playedRounds, firstPosition, rightToLeft);
                            }
                        }
                        else
                        {
                            SearchNodes(grid, currentNode, 1, start, letters, playedRounds, firstPosition, rightToLeft);
                        }
                    }

                }
                rightToLeft = false;
            }
        }
    }
    private bool CheckConnect(int grid, int j, int i)
    {
        if (board.GetSquare(grid, j - 1, i).Status == 1
           || board.GetSquare(grid, j + 1, i).Status == 1
           || board.GetSquare(grid, j, i + 1).Status == 1
           || board.GetSquare(grid, j, i - 1).Status == 1)
        {
            return true;
        }

        return false;
    }

    private void EndGame()
    {
#if DEBUG
        PrintGrid();
#endif
    }

    public void PrintGrid()
    {
#if DEBUG
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<link rel=\"stylesheet\" href=\"grid.css\" />");
        sb.AppendLine("</head>");

        sb.AppendLine("<body><div id='grid'>");

        sb.AppendLine("<table class='results' style='float:right'>");
        sb.AppendLine("<th><td>#</td><td>Rack</td><td>Word</td><td>pos</td><td>pts</td></th>");
        int ndx = 1;
        foreach (var r in currentGame.RoundsPlayed)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{ndx++}</td>");
            sb.AppendLine("<td></td>");
            sb.AppendLine($"<td>{r.GetDebugWord(true)}</td>");
            sb.AppendLine($"<td>{r.GetPosition()}</td>");
            sb.AppendLine($"<td>{r.Points}</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table>");

        sb.AppendLine("<table>");
        sb.AppendLine("<tr><td></td>");
        for (int col = 1; col < board.CurrentBoard[0].SizeH - 1; col++)
        {
            sb.AppendLine($"<td class='border'>{col}</td>");
        }
        sb.AppendLine("</tr>");

        for (int x = 1; x < board.CurrentBoard[0].SizeH - 1; x++)
        {
            var cc = ((char)(x + 64));
            sb.AppendLine($"<tr><td class='border'>{cc}</td>");
            for (int y = 1; y < board.CurrentBoard[0].SizeH - 1; y++)
            {
                var sq = board.GetSquare(0, y, x);
                var cclass = $"cell cell-{sq.LetterMultiplier} cell{sq.WordMultiplier}";

                if (sq.Status == 1)
                {
                    cclass += sq.CurrentLetter.IsJoker ? " tileJoker" : " tile";
                }

                sb.AppendLine($"<td class='{cclass}'>");
                if (sq.Status == 1)
                {
                    var c = (char)(sq.CurrentLetter.Letter + 97);
                    sb.AppendLine(char.ToUpper(c).ToString());
                }
                sb.AppendLine("</td>");
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");

        sb.AppendLine("</grid></body></html>");
        System.IO.File.WriteAllText("output.html", sb.ToString());
#endif
        // We are done 
    }

    private void SearchNodes(int grid, LetterNode parentNode, int increment, Position p, List<Tile> letters, PlayedRounds rounds, Position firstPosition, bool rightToLeft = true)
    {
        if (rounds.CurrentRound.Tiles.Count(p => p.Parent.Status == 0) >= currentGameConfig.PlayableLetters)
        {
            return;
        }

        int x = p.X;
        int y = p.Y;

        // We first Get the Square according to the cur
        //
        //
        // rent position
        var square = board.GetSquare(grid, x, y);


        // We load the nodes to be checked
        var nodes = new List<LetterNode>();

        if (square.IsBorder)
        {
            nodes = parentNode.Children.Where(p => p.Letter == TilesUtils.PivotByte).ToList();
        }
        else
        {
            nodes = parentNode.Children;
        }


        // We set the word/letter multipliers
        Tile tileLetter = new Tile();

        if (square != null && !parentNode.IsPivot)
        {
            if (square.Status == 1)
            {
                tileLetter = square.CurrentLetter;
                nodes = nodes.Where(p => p.Letter == tileLetter.Letter).ToList();
            }
        }

        // We go through each node
        foreach (var node in nodes)
        {
            // If node is a pivot we need to reset the traversal and invert the direction
            if (!node.IsPivot)
            {
                var letter = tileLetter;
                // The current square is empty so we can take a new letter from the rack
                if (square.Status == -1)
                {
                    if (letters.Any(p => p.Letter == node.Letter || p.IsJoker))
                    {
                        // if the letter is available in the rack or is a joker
                        letter = letters.FirstOrDefault(p => p.Letter == node.Letter || p.IsJoker);

                        // Is it possible to place this letter
                        if (!square.GetPivot(letter, grid, node.Letter))
                        {
                            continue;
                        }

                        letter.Mask = square.GetPivotPoints(grid);

                        // We remove the letter from the rack
                        int ndx = letters.FindIndex(p => p.Letter == node.Letter || p.IsJoker);
                        letters.RemoveAt(ndx);

                        // if the letter is a joker we asssign assign the current node letter
                        if (letter.IsJoker)
                        {
                            letter.Letter = node.Letter;
                        }

                        square.Status = 0;
                        rounds.CurrentRound.AddTile(letter, square);
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    rounds.CurrentRound.AddTile(letter, square);
                }

                // If the node isEnd we check the round 
                if (node.IsEnd)
                {
                    // For a round to be valid the next tile needs to be empty 
                    var nextTile = board.GetSquare(grid, x + increment, y);
                    if (nextTile.Status == -1)
                    {
                        // We update the position of the current word
                        if (firstPosition.ISGreater(p))
                        {
                            rounds.CurrentRound.Position = new Position(p);
                        }
                        else
                        {
                            rounds.CurrentRound.Position = new Position(firstPosition);
                        }


                        // We check the and calculate his score
                        rounds.SetRound(rounds.CurrentRound);
                        // We create a new round
                        rounds.CurrentRound = new PlayedRound(rounds.CurrentRound);
                    }
                }

                // if we reach the maximum number of playables we stop 
                var oldPosition = new Position(x + increment, y, grid);
                // We continue the search in the nodes 
                SearchNodes(grid, node, increment, oldPosition, letters, rounds, firstPosition);
                rounds.CurrentRound.Position = new Position(oldPosition);
                // We reset letter on the rack.
                rounds.CurrentRound.RemoveTile();

                // If letter comes from rack we put it back
                if (square.Status == 0)
                {
                    letters.Add(letter);
                    square.Status = -1;
                }
            }
            else
            {
                if (rightToLeft == true)
                {
                    rounds.CurrentRound.SetPivot();

                    Position pp = new Position(firstPosition.X - 1,
                        firstPosition.Y, grid);

                    SearchNodes(grid, node, -1, pp, letters, rounds, firstPosition);
                    rounds.CurrentRound.RemovePivot();
                }
            }

        }

    }
}