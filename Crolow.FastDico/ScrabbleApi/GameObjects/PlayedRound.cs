using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.ScrabbleApi;

public partial class ScrabbleAI
{
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

        public void AddTile(Tile tile, int wm, int lm)
        {
            tile.WordMultiplier = wm;
            tile.LetterMultiplier = lm;
            Tiles.Add(tile);
        }

        public Tile RemoveTile()
        {
            var t = Tiles[Tiles.Count - 1];
            Tiles.RemoveAt(Tiles.Count - 1);
            t.WordMultiplier = 1;
            t.LetterMultiplier = 1;

            return t;
        }

        public PlayedRound()
        {
            Tiles = new List<Tile>();
            Position = new Position(0, 0, 0);
        }

        public PlayedRound(PlayedRound copy)
        {
            Tiles = copy.Tiles.ToList();
            Pivot = copy.Pivot;
            Position = new Position(copy.Position);
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
            if (Position.Direction == 0)
            {
                return $"{(new char[] { ((char)(64 + Position.Y)) }[0])}{Position.X}";
            }

            return $"{Position.Y}{(new char[] { ((char)(64 + Position.X)) }[0])}";
        }


        /// <summary>
        /// We reset the tiles and create of them to keep the status
        /// </summary>
        public void FinalizeRound()
        {
            var l = Tiles.Take(Pivot);
            var m = Tiles.Skip(Pivot).ToList();
            if (Pivot != 0)
            {
                m.Reverse();
            }
            if (l.Count() > 0)
            {
                m.AddRange(l);
            }
            Tiles = m.Select(t => new Tile(t)).ToList();
            Pivot = 0;

        }


        public void DebugRound(string message)
        {
            string res = GetDebugWord();

            var txt = $"{message} : {res} "
                + Points + " : " + GetPosition();
            Console.WriteLine(txt);
        }

        public string GetDebugWord()
        {
            var l = Tiles.Take(Pivot).Select(p => p.Letter).ToList();
            var m = Tiles.Skip(Pivot).Select(p => p.Letter).ToList();
            if (Pivot != 0)
            {
                m.Reverse();
            }
            if (l.Count() > 0)
            {
                m.Add(31);
                m.AddRange(l);
            }

            string res = DawgUtils.ConvertBytesToWord(m);
            return res;
        }
    }
}