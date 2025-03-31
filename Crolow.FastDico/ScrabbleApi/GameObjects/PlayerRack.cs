using Crolow.FastDico.Search;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class PlayerRack
{
    public List<Tile> Tiles { get; private set; }

    public PlayerRack()
    {
        Tiles = new List<Tile>();
    }

    public void SetTiles(List<Tile> tiles)
    {
        Tiles = new List<Tile>();
    }

    public List<Tile> GetTiles()
    {
        return Tiles;
    }

    public void RemoveTile(Tile tile)
    {
        var i = Tiles.FindIndex(t => t.Letter == tile.Letter || (t.IsJoker && tile.IsJoker));
        if (i == -1)
        {
            Console.WriteLine("missing tile");
            return;
        }
        Tiles.RemoveAt(i);
    }


    // Example: Convert from human-readable string to byte representation
    public string ToString()
    {
        return TilesUtils.ConvertBytesToWord(Tiles.Select(p => p.Letter).ToList()); // Reuse existing utility
    }
}
