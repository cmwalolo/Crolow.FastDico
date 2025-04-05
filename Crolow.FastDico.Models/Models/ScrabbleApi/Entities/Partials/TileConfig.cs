namespace Crolow.FastDico.Models.Models.ScrabbleApi.Entities.Partials;

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
