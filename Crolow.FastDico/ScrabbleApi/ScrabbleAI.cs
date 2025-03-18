using Crolow.FastDico.Dawg;
using Crolow.FastDico.Dicos;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.Utils;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.ScrabbleApi.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics.Metrics;

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
        currentGameConfig = playConfiguration.SelectedConfig;
        GadDagCompiler gaddag = new GadDagCompiler();
        gaddag.ReadFromFile(playConfiguration.SelectedConfig.GaddagFile);
        this.currentGame = new CurrentGame { Configuration = playConfiguration };

        this.board = new Board(this.currentGame);
        this.letterBag = new LetterBag(this.currentGame);
        this.rack = new PlayerRack();
        this.dico = gaddag;
        this.gameConfig = playConfiguration.SelectedConfig;
        this.pivotBuilder = new PivotBuilder(board, gaddag.Root, playConfiguration);
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

        if (letters == null)
        {
            EndGame();
            return;
        }

        string res = DawgUtils.ConvertBytesToWord(letters);
        Console.WriteLine("Rack : " + res);
        Console.WriteLine("-------------------------------------------");


        var playedRounds = new PlayedRounds(gameConfig);

        // We set the original position to place which is at the board center
        if (firstMove)
        {
            Position p = new Position((board.CurrentBoard[0].SizeH - 1) / 2, (board.CurrentBoard[0].SizeV - 1) / 2, 0);
            playedRounds.CurrentRound.Position = new Position(p);
            SearchNodes(0, dico.Root, p, letters, 1, 0, playedRounds, p);
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
            if (letter.Status == 0)
            {
                rack.RemoveTile(letter);
            }
        }

        selectedRound.FinalizeRound();
        board.SetRound(selectedRound);
        currentGame.RoundsPlayed.Add(selectedRound);
        currentGame.Round++;
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
                var sq = board.GetSquare(grid, j, i);
                if (sq.CurrentLetter == null)
                {
                    if (CheckConnect(grid, j, i))
                    {
                        Position start = new Position(j, i, 0);
                        // If we are skipping only one square we do not need
                        // to search on the left.
                        rightToLeft = j == 1 || (j == oldj + 1 ? true : false);

                        // If there is a filled squared on the left
                        // We need to prefill the current solution
                        var sqLeft = board.GetSquare(grid, j - 1, i);
                        if (sqLeft.CurrentLetter != null && sqLeft.CurrentLetter.Status == 1)
                        {
                            var sql = new List<Square>();
                            sql.Add(sqLeft);
                            var pos = j - 2;
                            while (true)
                            {
                                var sqNext = board.GetSquare(grid, pos, i);
                                if (sqNext.CurrentLetter != null && sqNext.CurrentLetter.Status == 1)
                                {
                                    sql.Add(sqNext);
                                    pos--;
                                }
                                else
                                {
                                    sql.Reverse();
                                    playedRounds.CurrentRound = new PlayedRound();

                                    foreach (var item in sql)
                                    {
                                        playedRounds.CurrentRound.AddTile(item.CurrentLetter, 1, 1);
                                    }
                                    break;
                                }
                            }
                        }

                        // Ok we can process that square
                        SearchNodes(grid, dico.Root, start, letters, 1, 0, playedRounds, start, rightToLeft);
                    }

                }
            }
        }
    }
    private bool CheckConnect(int grid, int j, int i)
    {
        if (board.GetSquare(grid, j - 1, i).CurrentLetter != null
           || board.GetSquare(grid, j + 1, i).CurrentLetter != null
           || board.GetSquare(grid, j, i + 1).CurrentLetter != null
           || board.GetSquare(grid, j, i - 1).CurrentLetter != null)
        {
            return true;
        }

        return false;
    }

    private void EndGame()
    {
        // We are done 
    }

    private void SearchNodes(int grid, LetterNode parentNode, Position p, List<Tile> letters, int incH, int incV, PlayedRounds rounds, Position FirstPosition, bool rightToLeft = true)
    {
        if (rounds.CurrentRound.Tiles.Count(p => p.Status == 0) >= currentGameConfig.PlayableLetters)
        {
            return;
        }

        int x = p.X;
        int y = p.Y;

        // We first Get the Square according to the current position
        var square = board.GetSquare(grid, x, y);

        // We define the dirrection
        int direction = 0;

        // We load the nodes to be checked
        var nodes = new List<LetterNode>();

        if (square == null || square.IsBorder)
        {
            nodes = parentNode.Children.Where(p => p.Letter == DawgUtils.PivotByte).ToList();
        }
        else
        {
            nodes = parentNode.Children;
        }


        // We set the word/letter multipliers
        var wm = 1;
        var lm = 1;
        Tile tileLetter = null;

        if (square != null && !parentNode.IsPivot)
        {
            tileLetter = square.CurrentLetter;
            wm = square.CurrentLetter == null ? square.WordMultiplier : 1;
            lm = square.CurrentLetter == null ? square.LetterMultiplier : 1;
            if (square.CurrentLetter != null)
            {
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
                if (letter == null)
                {
                    letter = letters.FirstOrDefault(p => p.Letter == node.Letter || p.IsJoker);
                    // if the letter is available in the rack or is a joker
                    if (letter != null)
                    {
                        if (!square.GetPivot(letter.Letter, direction) && !letter.IsJoker)
                        {
                            continue;
                        }
                        // We remove the letter from the rack
                        letters.Remove(letter);

                        // if the letter is a joker we asssign assign the current node letter
                        if (letter.IsJoker)
                        {
                            letter.Letter = node.Letter;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                // We add a letter to round if not null
                if (letter != null)
                {
                    // We set a new tile 
                    rounds.CurrentRound.AddTile(letter, wm, lm);

                    // If the node isEnd we check the round 
                    if (node.IsEnd)
                    {
                        // For a round to be valid the next tile needs to be empty 
                        var nextTile = board.GetSquare(grid, x + incH, y + incV);
                        if (nextTile.CurrentLetter == null)
                        {
                            // We set the final position of the round
                            if (FirstPosition.ISGreater(p))
                            {
                                rounds.CurrentRound.Position = p;
                            }
                            else
                            {
                                rounds.CurrentRound.Position = FirstPosition;
                            }
                            // We check the and calculate his score
                            rounds.SetRound(rounds.CurrentRound);
                            // We create a new round
                            rounds.CurrentRound = new PlayedRound(rounds.CurrentRound);
                        }
                    }

                    // if we reach the maximum number of playables we stop 
                    var oldPosition = new Position(x + incH, y + incV, direction);
                    // We continue the search in the nodes 
                    SearchNodes(grid, node, oldPosition, letters, incH, incV, rounds, FirstPosition);
                    rounds.CurrentRound.Position = new Position(oldPosition);
                    // We reset letter on the rack.

                    rounds.CurrentRound.RemoveTile(wm);

                    // If letter comes from rack we put it back
                    if (letter.Status == 0)
                    {
                        letters.Add(letter);
                    }
                }
            }
            else
            {
#if DEBUG
                if (DawgUtils.ConvertBytesToWord(rounds.CurrentRound.Tiles) == "aquero")
                {
                    rounds.CurrentRound.DebugRound("DEBUG");
                }
#endif

                rounds.CurrentRound.SetPivot();

                Position pp = new Position(FirstPosition.X - incH,
                    FirstPosition.Y - incV, direction);

                SearchNodes(grid, node, pp, letters, -incH, -incV, rounds, FirstPosition);
                rounds.CurrentRound.RemovePivot();
                rounds.CurrentRound.Position = new Position(pp);
            }

        }

    }
}