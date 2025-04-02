namespace Crolow.FastDico.ScrabbleApi.GameObjects;
public struct Tile
{
    public Tile()
    {
    }

    public Tile(Tile tile, Square parent)
    {
        Letter = tile.Letter;
        IsVowel = tile.IsVowel;
        IsConsonant = tile.IsConsonant;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
        TotalLetters = tile.TotalLetters;
        Parent = tile.Parent;
    }

#if DEBUG
    public int x { get; set; }
#endif

    public Square Parent { get; set; }
    public int Mask { get; set; }
    public byte Letter { get; set; }
    public bool IsVowel { get; set; }
    public bool IsConsonant { get; set; }
    public int TotalLetters { get; set; }
    public int Points { get; set; }
    public bool IsJoker { get; set; }
}
