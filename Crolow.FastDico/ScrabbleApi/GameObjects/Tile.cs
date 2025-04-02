namespace Crolow.FastDico.ScrabbleApi.GameObjects;
public struct Tile
{
    public Tile()
    {
        Status = -1;
    }

    public Tile(Tile tile, int status = -1)
    {
        Letter = tile.Letter;
        IsVowel = tile.IsVowel;
        IsConsonant = tile.IsConsonant;
        Status = status == -1 ? tile.Status : 0;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
        WordMultiplier = tile.LetterMultiplier;
        LetterMultiplier = tile.WordMultiplier;
        TotalLetters = tile.TotalLetters;

    }

#if DEBUG
    public int x { get; set; }
#endif
    public int Mask { get; set; }

    public int LetterMultiplier { get; set; }
    public int WordMultiplier { get; set; }

    public byte Letter { get; set; }
    public bool IsVowel { get; set; }
    public bool IsConsonant { get; set; }
    public int TotalLetters { get; set; }
    public int Points { get; set; }
    public bool IsJoker { get; set; }
    public int Status { get; set; }
}
