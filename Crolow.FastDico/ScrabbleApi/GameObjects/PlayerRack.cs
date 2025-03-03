using Crolow.Fast.Dawg.Utils;
using Crolow.FastDico.ScrabbleApi.Config;

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


    // Example: Convert from human-readable string to byte representation
    public string ToString()
    {
        return DawgUtils.ConvertBytesToWord(Tiles.Select(p => p.Letter).ToList()); // Reuse existing utility
    }
}
