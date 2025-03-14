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
    private PlayConfiguration PlayConfiguration;
    private Board board;
    private GameConfig gameConfig;
    private LetterBag letterBag;
    private DawgSearch gaddag;
    private PlayerRack rack;
    private CurrentGame currentGame;
    private GameConfig currentGameConfig;

    public ScrabbleAI(string configsFile, string configName)
    {
        PlayConfiguration = new ConfigReader().ReadConfiguration(configsFile, configName);
        currentGameConfig = PlayConfiguration.SelectedConfig;
        GadDagCompiler gaddag = new GadDagCompiler();
        gaddag.ReadFromFile(PlayConfiguration.SelectedConfig.GaddagFile);
        this.currentGame = new CurrentGame { Configuration = PlayConfiguration };

        this.board = new Board(this.currentGame);
        this.letterBag = new LetterBag(this.currentGame);
        this.rack = new PlayerRack();
        this.dico = gaddag;
        this.gameConfig = PlayConfiguration.SelectedConfig;
    }

    public void StartGame()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        NextRound(true);
        stopwatch.Stop();
        Console.WriteLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

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


        var t = letters.Select(p => p.Letter).ToList();

        var playedRounds = new PlayedRounds(gameConfig);

        // We set the original position to place which is at the board center
        if (firstMove)
        {
            Position p = new Position((board.CurrentBoard.SizeH - 1) / 2, (board.CurrentBoard.SizeV - 1) / 2, 0);
            playedRounds.CurrentRound.Position = new Position(p);
            SearchNodes(dico.Root, p, letters, 1, 0, playedRounds, p);
        }

        var selectedRound = playedRounds.Rounds.FirstOrDefault();
        if (selectedRound == null)
        {
            EndGame(); return;
        }

        var tiles = selectedRound.ReorderTiles();


    }

    private void EndGame()
    {
        // We are done 
    }

    private void SearchNodes(LetterNode parentNode, Position p, List<Tile> letters, int incH, int incV, PlayedRounds rounds, Position FirstPosition)
    {
        if (rounds.CurrentRound.Tiles.Count(p => p.Status == 0) >= currentGameConfig.PlayableLetters)
        {
            return;
        }

        int x = p.X;
        int y = p.Y;

        // We first Get the Square according to the current position
        var square = board.GetSquare(x, y);

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
                    rounds.CurrentRound.SetTile(letter, wm, lm);

                    // If the node isEnd we check the round 
                    if (node.IsEnd)
                    {
                        // For a round to be valid the next tile needs to be empty 
                        var nextTile = board.GetSquare(x + incH, y + incV);
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
                    SearchNodes(node, oldPosition, letters, incH, incV, rounds, FirstPosition);
                    rounds.CurrentRound.Position = new Position(oldPosition);
                    // We reset letter on the rack.
                    letters.Add(rounds.CurrentRound.RemoveTile(wm));
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

                SearchNodes(node, pp, letters, -incH, -incV, rounds, FirstPosition);
                rounds.CurrentRound.RemovePivot();
                rounds.CurrentRound.Position = new Position(pp);
            }

        }

    }
}