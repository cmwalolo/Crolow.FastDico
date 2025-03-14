using Crolow.FastDico.ScrabbleApi.Config;
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

        public Tile RemoveTile(int m)
        {
            var t = Tiles[Tiles.Count - 1];
            Tiles.RemoveAt(Tiles.Count - 1);
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
            return $"{(new char[] { ((char)(64 + Position.Y)) }[0])}{Position.X}";
        }


        public List<Tile> ReorderTiles()
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

            return m;
        }


        public void DebugRound(string message)
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
            var txt = $"{message} : {res} "
                + Points + " : " + GetPosition();
            Console.WriteLine(txt);
        }
    }
}