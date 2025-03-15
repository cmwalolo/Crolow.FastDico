using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.Utils;

public static class DawgUtils
{
    public const byte IsEnd = 1;
    public const byte PivotByte = 31;

    // Convert a string (lowercase letters) to a byte array
    public static List<byte> ConvertWordToBytes(string word)
    {
        List<byte> byteArray = new List<byte>();
        foreach (var letter in word)
        {

            byteArray.Add(letter == '#' ? (byte)31 : (byte)(letter - 'a'));
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
            wordChars[i] = b == 31 ? '#' : (b == 30 ? '?' : (char)(byteArray[i] + 'a'));
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
}
