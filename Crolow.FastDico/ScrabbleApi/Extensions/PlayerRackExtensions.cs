using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.ScrabbleApi.Extensions;

public static class PlayerRackExtensions
{
    public static void SetTiles(this PlayerRack r, List<Tile> tiles)
    {
        r.Tiles = new List<Tile>();
    }

    public static List<Tile> GetTiles(this PlayerRack r)
    {
        return r.Tiles;
    }

    public static void RemoveTile(this PlayerRack r, Tile tile)
    {
        var i = r.Tiles.FindIndex(t => t.Letter == tile.Letter || t.IsJoker && tile.IsJoker);
        if (i == -1)
        {
            Console.WriteLine("missing tile");
            return;
        }
        r.Tiles.RemoveAt(i);
    }


    // Example: Convert from human-readable string to byte representation
    public static string GetString(this PlayerRack r)
    {
        return TilesUtils.ConvertBytesToWord(r.Tiles.Select(p => p.IsJoker ? TilesUtils.JokerByte : p.Letter).ToList()); // Reuse existing utility
    }
    public static void Clear(this PlayerRack r)
    {
        r.Tiles = new List<Tile>();
    }
}
