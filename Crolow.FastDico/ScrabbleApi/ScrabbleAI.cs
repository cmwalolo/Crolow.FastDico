using Crolow.Fast.Dawg.Dawg;
using Crolow.Fast.Dawg.Dicos;
using Crolow.Fast.Dawg.GadDag;
using Crolow.Fast.Dawg.Utils;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

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

    public class PlayedRounds
    {
        PlayConfig Config { get; set; }
        public int MaxPoints { get; set; }
        public List<PlayedRound> Rounds { get; set; }
        public PlayedRound CurrentRound { get; set; }
        public PlayedRounds(PlayConfig config)
        {
            Config = config;
            Rounds = new List<PlayedRound>();
            CurrentRound = new PlayedRound();
        }
        public void SetRound()
        {
            int wm = 1;
            foreach (var t in CurrentRound.Tiles)
            {
                wm *= t.WordMultiplier;
            }

            CurrentRound.Points = CurrentRound.Tiles.Sum(p => p.Points * p.LetterMultiplier) * wm;

            var tilesFromRack = CurrentRound.Tiles.Count(p => p.Status == 0);
            if (tilesFromRack > 0 && tilesFromRack < Config.Bonus.Count())
            {
                CurrentRound.Bonus = Config.Bonus[tilesFromRack - 1];
            }

            CurrentRound.Points += CurrentRound.Bonus;

            if (CurrentRound.Points > MaxPoints)
            {
                Rounds.Clear();
                Rounds.Add(CurrentRound);
                MaxPoints = CurrentRound.Points;
            }
            else
            {
                return;
            }


#if DEBUG

            var l = CurrentRound.Tiles.Take(CurrentRound.Pivot).Select(p => p.Letter).ToList();
            var m = CurrentRound.Tiles.Skip(CurrentRound.Pivot).Select(p => p.Letter).ToList();
            if (CurrentRound.Pivot != 0)
            {
                m.Reverse();
            }
            if (l.Count() > 0)
            {
                m.Add(31);
                m.AddRange(l);
            }

            string res = DawgUtils.ConvertBytesToWord(m);
            var txt = $"Word found : {res} "
                + CurrentRound.Points + " : " + CurrentRound.GetPosition();
            Console.WriteLine(txt);

#endif
        }

    }

    public class PlayedRound
    {
        public List<Tile> Tiles { get; set; }
        public Position Position { get; set; }
        public int Points { get; set; }
        public int PlayedTime { get; set; }
        public int Bonus { get; set; }
        public int Pivot { get; set; }

        public void SetTile(Tile tile, int wm, int lm)
        {
            Tile t = new Tile(tile);
            t.WordMultiplier = wm;
            t.LetterMultiplier = lm;
            Tiles.Add(t);
        }

        public Tile RemoveTile(int m)
        {
            var t = Tiles[Tiles.Count - 1];
            Tiles.RemoveAt(Tiles.Count - 1);
            return new Tile(t);
        }

        public PlayedRound()
        {
            Tiles = new List<Tile>();
            Position = new Position(0, 0, 0);
        }

        public PlayedRound(PlayedRound copy)
        {
            Tiles = copy.Tiles;
            Pivot = copy.Pivot;
            Position = copy.Position;
        }

        internal void SetPivot()
        {
            Pivot = Tiles.Count;
        }

        internal void RemovePivot()
        {
            Pivot = 0;
        }

        internal string GetPosition()
        {
            return $"{(new char[] { ((char)(64 + Position.Y)) }[0])}{Position.X}";
        }
    }

    public class CurrentGame
    {
        public int Round { get; set; }
        public int TotalPoints { get; set; }
        public int PlayTime { get; set; }

    }

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
        //        var letters = letterBag.DrawLetters(playConfig.InRackLetters, rack);
        var letters = letterBag.ForceDrawLetters("lecparaism");
        string res = DawgUtils.ConvertBytesToWord(letters);
        Console.WriteLine("Rack : " + res);
        Console.WriteLine("-------------------------------------------");


        var t = letters.Select(p => p.Letter).ToList();

        Position p = new Position((board.CurrentBoard.SizeH - 1) / 2, (board.CurrentBoard.SizeV - 1) / 2, 0);
        var playedRounds = new PlayedRounds(playConfig);
        playedRounds.CurrentRound.Position = new Position(p);
        SearchNodes(dico.Root, p.X, p.Y, letters, 1, 0, playedRounds, p);
    }
    private void SearchNodes(LetterNode parentNode, int x, int y, List<Tile> letters, int incH, int incV, PlayedRounds rounds, Position FirstPosition)
    {
        if (x < 1 || y < 1)
        {
            return;
        }

        // We are moving backwards so we adjust the round
        if (incV < 0 || incH < 0)
        {
            rounds.CurrentRound.Position.X = x;
            rounds.CurrentRound.Position.Y = y;
        }

        int direction = x != 0 ? 0 : 1;

        var square = board.GetTile(x, y);

        var nodes = new List<LetterNode>();

        if (square == null || square.IsBorder)
        {
            nodes = parentNode.Children.Where(p => p.Letter == DawgUtils.PivotByte).ToList();
        }
        else
        {
            nodes = parentNode.Children;
        }

        var wm = 1;
        var lm = 1;
        Tile tileLetter = null;

        if (square != null && !parentNode.IsPivot)
        {
            tileLetter = square.CurrentLetter;
            wm = square.CurrentLetter == null ? square.WordMultiplier : 1;
            lm = square.CurrentLetter == null ? square.LetterMultiplier : 1;
        }

        foreach (var node in nodes)
        {
            if (!node.IsPivot)
            {
                var letter = tileLetter;

                // The current square is empty 
                if (letter == null)
                {
                    letter = letters.FirstOrDefault(p => p.Letter == node.Letter || p.IsJoker);
                    if (letter != null)
                    {
                        if (!square.GetPivot(letter.Letter, direction) && !letter.IsJoker)
                        {
                            continue;
                        }
                        letters.Remove(letter);

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

                // We add a letter to the round
                if (letter != null)
                {
                    rounds.CurrentRound.SetTile(letter, wm, lm);

                    if (DawgUtils.ConvertBytesToWord(rounds.CurrentRound.Tiles) == "mplacais")
                    {
                        Console.WriteLine("ok");
                    }

                    if (node.IsEnd)
                    {
                        if (x + incH > 15)
                        {
                            // Console.WriteLine("ok end of grid");
                        }
                        var nextTile = board.GetTile(x + incH, y + incV);
                        if (nextTile.CurrentLetter == null)
                        {
                            rounds.SetRound();

                            var tiles = rounds.CurrentRound.Tiles;
                            var position = rounds.CurrentRound.Position;
                            var pivot = rounds.CurrentRound.Pivot;
                            rounds.CurrentRound = new PlayedRound()
                            {
                                Tiles = tiles,
                                Position = position,
                                Pivot = pivot
                            };
                        }
                    }

                    SearchNodes(node, x + incH, y + incV, letters, incH, incV, rounds, FirstPosition);

                    letters.Add(rounds.CurrentRound.RemoveTile(wm));
                }
            }
            else
            {
                rounds.CurrentRound.SetPivot();
                SearchNodes(node, FirstPosition.X - incH, FirstPosition.Y - incV, letters, -incH, -incV, rounds, FirstPosition);
                rounds.CurrentRound.RemovePivot();
                rounds.CurrentRound.Position = new Position(FirstPosition);
            }

        }

    }
}