namespace Crolow.Fast.Dawg.ScrabbleApi;

public record Tile(byte Letter, TileMultiplier Multiplier)
{
    public bool IsOccupied => Letter != 0;

    public Tile(TileMultiplier multiplier = TileMultiplier.None) : this(0, multiplier)
    {
        // Empty tile

    }
}
