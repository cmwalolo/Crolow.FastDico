namespace Crolow.FastDico.ScrabbleApi.GameObjects;
public class Tile
{
    public Tile()
    {

    }

    public Tile(Tile tile, int status = 0)
    {
        Letter = tile.Letter;
        IsVowel
            = tile.IsVowel;
        IsConsonant = tile.IsConsonant;
        Status = status;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
        WordMultiplier = tile.LetterMultiplier;
        LetterMultiplier = tile.WordMultiplier;
    }

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
