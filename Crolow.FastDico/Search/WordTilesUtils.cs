using Crolow.FastDico.Utils;

namespace Crolow.FastDico.Search;

public static class WordTilesUtils
{
    public static string ConvertBytesToWordForDisplay(WordResults.Word word)
    {
        char[] wordChars = new char[word.Tiles.Count];
        for (int i = 0; i < word.Tiles.Count; i++)
        {
            var b = word.Tiles[i];
            wordChars[i] = b.IsJoker ? (char)(b.Letter + 'a') : (char)(b.Letter + 'A');
        }
        return new string(wordChars);
    }

    public static string ConvertBytesToWordByStatus(WordResults.Word word, int status)
    {
        var tiles = word.Tiles.Where(p => p.Status == status).OrderBy(p => p.Letter).ToArray();
        char[] wordChars = new char[tiles.Count()];
        for (int i = 0; i < tiles.Count(); i++)
        {
            var b = tiles[i];
            wordChars[i] = b.IsJoker ? (char)(b.Letter + 'a') : (char)(b.Letter + 'A');
        }
        return new string(wordChars);
    }

    public static List<WordResults.Tile> ConvertWordToBytes(string word, string optional)
    {
        List<WordResults.Tile> byteArray = new List<WordResults.Tile>();
        foreach (var letter in word)
        {
            switch (letter)
            {
                case '?':
                    byteArray.Add(new WordResults.Tile(TilesUtils.JokerByte, true, 0));
                    break;
                case '*':
                    byteArray.Add(new WordResults.Tile(TilesUtils.WildcardByte, true, 0));
                    break;
                default:
                    byteArray.Add(new WordResults.Tile((byte)(letter - 'a'), false, 0));
                    break;
            }
        }

        foreach (var letter in optional)
        {
            switch (letter)
            {
                case '?':
                    byteArray.Add(new WordResults.Tile(TilesUtils.JokerByte, true, 1));
                    break;
                default:
                    byteArray.Add(new WordResults.Tile((byte)(letter - 'a'), false, 1));
                    break;
            }
        }
        return byteArray;
    }
}
