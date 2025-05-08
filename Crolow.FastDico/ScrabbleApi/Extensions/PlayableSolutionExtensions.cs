using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.ScrabbleApi.Extensions;

public static class PlayableSolutionExtensions
{
    public static void AddTile(this PlayableSolution s, Tile tile, Square sq)
    {
        tile.Parent = sq;
        s.Tiles.Add(tile);
    }

    public static Tile RemoveTile(this PlayableSolution s)
    {
        var t = s.Tiles[s.Tiles.Count - 1];
        s.Tiles.RemoveAt(s.Tiles.Count - 1);
        return t;
    }

    public static void SetPivot(this PlayableSolution s)
    {
        s.Pivot = s.Tiles.Count;
    }

    public static void RemovePivot(this PlayableSolution s)
    {
        s.Pivot = 0;
    }

    public static string GetPosition(this PlayableSolution s)
    {
        if (s.Position.Direction == 0)
        {
            return $"{new char[] { (char)(64 + s.Position.Y) }[0]}{s.Position.X}";
        }

        return $"{s.Position.X}{new char[] { (char)(64 + s.Position.Y) }[0]}";
    }


    /// <summary>
    /// We reset the tiles and create of them to keep the status
    /// </summary>
    public static void FinalizeRound(this PlayableSolution s)
    {
        var l = s.Tiles.Take(s.Pivot);
        var m = s.Tiles.Skip(s.Pivot).ToList();
        if (s.Pivot != 0)
        {
            m.Reverse();
        }
        if (l.Count() > 0)
        {
            m.AddRange(l);
        }
        s.Tiles = m.Select(t => new Tile(t, t.Parent)).ToList();
        s.Pivot = 0;

        // If we played vertically, we reset the position
        // To the transposed coordinate
        if (s.Position.Direction == 1)
        {
            s.Position = new Position(s.Position.Y, s.Position.X, 1);
        }
    }
    public static void DebugRound(this PlayableSolution s, string message)
    {
        string res = s.GetWord(true);
        string resRaw = s.GetWord(false);

        var txt = $"{message} : {res} {s.Points} : {s.GetPosition()} - {resRaw}";
        Console.WriteLine(txt);
    }

    public static string GetWord(this PlayableSolution s, bool reorder = true)
    {
        if (reorder)
        {
            var l = s.Tiles.Take(s.Pivot);
            var m = s.Tiles.Skip(s.Pivot).ToList();
            if (s.Pivot != 0)
            {
                m.Reverse();
            }
            if (l.Count() > 0)
            {
                m.AddRange(l);
            }
            return TilesUtils.ConvertBytesToWord(m);
        }
        else
        {
            var m = s.Tiles;
            return TilesUtils.ConvertBytesToWord(m);
        }
    }

    public static string ToString(this PlayableSolution s)
    {
        return s.GetWord(true);
    }

}
