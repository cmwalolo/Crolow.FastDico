using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.Utils;

public static class TilesUtils
{
    public const byte IsEnd = 1;
    public const byte PivotByte = 31;
    public const byte WildcardByte = 32;
    public const byte SingleByte = 33;
    public const byte JokerByte = 30;

    // Convert a string (lowercase letters) to a byte array
    public static List<byte> ConvertWordToBytes(string word)
    {
        List<byte> byteArray = new List<byte>();
        foreach (var letter in word)
        {
            switch (letter)
            {
                case '#':
                    byteArray.Add(31);
                    break;
                case '?':
                    byteArray.Add(TilesUtils.JokerByte);
                    break;
                default:
                    byteArray.Add((byte)(letter - 'a'));
                    break;
            }
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
            wordChars[i] = b == 31 ? '#' : (b == TilesUtils.JokerByte ? '?' : (char)(byteArray[i] + 'a'));
        }
        return new string(wordChars);
    }

    public static string ConvertBytesToWord(List<Tile> m)
    {
        char[] wordChars = new char[m.Count];
        for (int i = 0; i < m.Count; i++)
        {
            wordChars[i] = (char)(m[i].Letter == 31 ? '#' : m[i].IsJoker ? ((char)m[i].Letter + 'A') : ((char)m[i].Letter + 'a'));
        }
        return new string(wordChars).Replace('[', '?');
    }

    public static string ConvertBytesToWordForDisplay(List<byte> byteArray, List<byte> jokers = null)
    {
        char[] wordChars = new char[byteArray.Count];
        if (jokers == null)
        {
            for (int i = 0; i < byteArray.Count; i++)
            {
                byte b = byteArray[i];
                wordChars[i] = (char)(byteArray[i] + 'A');
            }
        }
        else
        {
            for (int i = 0; i < byteArray.Count; i++)
            {
                byte b = byteArray[i];
                wordChars[i] = jokers[i] == 1 ? (char)(byteArray[i] + 'a') : (char)(byteArray[i] + 'A');
            }
        }
        return new string(wordChars);
    }

}
