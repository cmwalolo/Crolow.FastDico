namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public struct TileConfig
{
    public TileConfig()
    {
    }

    public char Char { get; set; }
    public byte Letter { get; set; }
    public bool IsVowel { get; set; }
    public bool IsConsonant { get; set; }
    public int TotalLetters { get; set; }
    public int Points { get; set; }
    public bool IsJoker { get; set; }
}

public struct Tile
{
    public Tile()
    {
    }

    public Tile(TileConfig tile, Square parent)
    {
        Letter = tile.Letter;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
    }

    public Tile(Tile tile, Square parent)
    {
        Letter = tile.Letter;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
        Parent = tile.Parent;
    }


    public Square Parent { get; set; }
    public int Mask { get; set; }
    public byte Letter { get; set; }
    public int Points { get; set; }
    public bool IsJoker { get; set; }
}
