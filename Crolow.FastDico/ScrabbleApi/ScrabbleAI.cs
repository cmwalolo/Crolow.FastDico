using Crolow.Fast.Dawg.Dawg;
using Crolow.Fast.Dawg.Dicos;
using Crolow.Fast.Dawg.GadDag;
using Crolow.Fast.Dawg.Utils;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.Fast.Dawg.ScrabbleApi;

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
    private GameConfiguration GameConfiguration;
    private Board board;
    private PlayConfig playConfig;
    private LetterBag letterBag;
    private DawgSearch gaddag;
    private PlayerRack rack;
    private CurrentGame currentGame;

    public ScrabbleAI(GameConfiguration config, GadDagCompiler gaddag)
    {
        this.board = new Board(config);
        this.letterBag = new LetterBag(config);
        this.playConfig = config.PlayConfig.Configurations[0];
        this.rack = new PlayerRack();
        this.dico = gaddag;
    }

    public void StartGame()
    {
        DoFirstMove();
    }
    private void DoFirstMove()
    {
        //var letters = letterBag.DrawLetters(playConfig.InRackLetters, rack);
        // Used for testing 
        var letters = letterBag.ForceDrawLetters("vaquerabou");
        string res = DawgUtils.ConvertBytesToWord(letters);
        Console.WriteLine("Rack : " + res);
        Console.WriteLine("-------------------------------------------");


        var t = letters.Select(p => p.Letter).ToList();

        var playedRounds = new PlayedRounds(playConfig);

        // We set the original position to place which is at the board center
        Position p = new Position((board.CurrentBoard.SizeH - 1) / 2, (board.CurrentBoard.SizeV - 1) / 2, 0);
        playedRounds.CurrentRound.Position = new Position(p);
        SearchNodes(dico.Root, p, letters, 1, 0, playedRounds, p);
    }
    private void SearchNodes(LetterNode parentNode, Position p, List<Tile> letters, int incH, int incV, PlayedRounds rounds, Position FirstPosition)
    {
        int x = p.X;
        int y = p.Y;

        // We first Get the Square according to the current position
        var square = board.GetTile(x, y);

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

                    if (DawgUtils.ConvertBytesToWord(rounds.CurrentRound.Tiles) == "aquav")
                    {
                        rounds.DebugRound(rounds.CurrentRound);
                    }

                    // If the node isEnd we check the round 
                    if (node.IsEnd)
                    {
                        if (x + incH > 15)
                        {
                            // Console.WriteLine("ok end of grid");
                        }

                        // For a round to be valid the next tile needs to be empty 
                        var nextTile = board.GetTile(x + incH, y + incV);
                        if (nextTile.CurrentLetter == null)
                        {
                            // We check the and calculate his score
                            rounds.SetRound(rounds.CurrentRound);
                            // We create a new round
                            rounds.CurrentRound = new PlayedRound(rounds.CurrentRound);
                        }
                    }

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
                if (DawgUtils.ConvertBytesToWord(rounds.CurrentRound.Tiles) == "aqua")
                {
                    rounds.DebugRound(rounds.CurrentRound);
                }

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