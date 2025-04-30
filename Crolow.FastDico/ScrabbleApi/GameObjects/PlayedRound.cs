using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.ScrabbleApi;

public class PlayedRound
{
    public List<Tile> Tiles { get; set; }
    public Position Position { get; set; }
    public int Points { get; set; }
    public int PlayedTime { get; set; }
    public int Bonus { get; set; }
    public int Pivot { get; set; }

    public PlayerRack Rack { get; set; }


    public void SetTile(Tile tile, Square sq)
    {
        tile.Parent = sq;
        Tiles.Add(tile);
    }

    public void AddTile(Tile tile, Square sq)
    {
        tile.Parent = sq;
        Tiles.Add(tile);
    }

    public Tile RemoveTile()
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

    public string GetPosition()
    {
        if (Position.Direction == 0)
        {
            return $"{(new char[] { ((char)(64 + Position.Y)) }[0])}{Position.X}";
        }

        return $"{Position.X}{(new char[] { ((char)(64 + Position.Y)) }[0])}";
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
        Tiles = m.Select(t => new Tile(t, t.Parent)).ToList();
        Pivot = 0;

        // If we played vertically, we reset the position
        // To the transposed coordinate
        if (Position.Direction == 1)
        {
            Position = new Position(Position.Y, Position.X, 1);
        }
    }


    public void DebugRound(string message)
    {
        string res = GetWord(true);
        string resRaw = GetWord(false);

        var txt = $"{message} : {res} {Points} : {GetPosition()} - {resRaw}";
        Console.WriteLine(txt);
    }

    public string GetWord(bool reorder = true)
    {
        if (reorder)
        {
            var l = Tiles.Take(Pivot).Select(p => p.Letter).ToList();
            var m = Tiles.Skip(Pivot).Select(p => p.Letter).ToList();
            if (Pivot != 0)
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
            var m = Tiles.Select(p => p.Letter).ToList();
            return TilesUtils.ConvertBytesToWord(m);
        }
    }

    public string ToString()
    {
        return GetWord(true);
    }

}
