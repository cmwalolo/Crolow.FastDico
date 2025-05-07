using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.FastDico.Utils;

public static class TilesUtils
{
    public static BagConfiguration configuration { get; set; }

    public const byte IsEnd = 1;
    public const byte PivotByte = 255;          // "#"
    public const byte WildcardByte = 254;       // "*"
    public const byte SingleMatchByte = 253;    // "? => for search purpose"
    public const byte JokerByte = 252;          // "?" 

    // Convert a string (lowercase letters) to a byte array
    public static List<byte> ConvertWordToBytes(string word)
    {
        List<byte> byteArray = new List<byte>();
        try
        {
            foreach (var letter in word)
            {
                switch (letter)
                {
                    case '#':
                        byteArray.Add(TilesUtils.PivotByte);
                        break;
                    case '?':
                        byteArray.Add(TilesUtils.JokerByte);
                        break;
                    case '*':
                        byteArray.Add(TilesUtils.WildcardByte);
                        break;
                    default:
                        byteArray.Add(configuration.LettersByChar[letter].Letter);
                        break;
                }
            }
        }
        catch (Exception)
        {
            ;
        }
        return byteArray;
    }

    // Convert a byte array back to a string
    public static string ConvertBytesToWord(List<byte> byteArray)
    {
        char[] wordChars = new char[byteArray.Count];
        for (int i = 0; i < byteArray.Count; i++)
        {
            byte b = byteArray[i];
            wordChars[i] = b == PivotByte ? '#' : (b == TilesUtils.JokerByte ? '?' : configuration.LettersByByte[b].Char[0]);
        }
        return new string(wordChars);
    }

    public static string ConvertBytesToWord(List<Tile> m)
    {
        char[] wordChars = new char[m.Count];
        for (int i = 0; i < m.Count; i++)
        {
            var c = configuration.LettersByByte[m[i].Letter].Char;
            wordChars[i] = (char)(m[i].Letter == PivotByte ? '#' : (m[i].IsJoker ? (m[i].Letter == JokerByte ? '?' : char.ToLower(c[0])) : c[0]));
        }
        return new string(wordChars);
    }

    public static string ConvertBytesToWordForDisplay(List<byte> byteArray, List<byte> jokers = null)
    {
        char[] wordChars = new char[byteArray.Count];
        if (jokers == null)
        {
            for (int i = 0; i < byteArray.Count; i++)
            {
                byte b = byteArray[i];
                wordChars[i] = b == JokerByte ? '?' : configuration.LettersByByte[b].Char[0];
            }
        }
        else
        {
            for (int i = 0; i < byteArray.Count; i++)
            {
                byte b = byteArray[i];
                var c = configuration.LettersByByte[b].Char;
                wordChars[i] = jokers[i] == 1 ? char.ToLower(c[0]) : c[0];
            }
        }
        return new string(wordChars);
    }

}
