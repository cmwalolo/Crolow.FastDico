using Crolow.FastDico.Dicos;
using Crolow.FastDico.Search;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.GadDag;
public class GadDagSearchCore
{
    public LetterNode Root { get; private set; }

    public GadDagSearchCore(LetterNode root)
    {
        Root = root;
    }



    // Function 1: Find all words that can be formed using exactly the given letters
    public WordResults FindAllWordsFromLetters(string pattern, string optional)
    {
        var letters = WordTilesUtils.ConvertWordToBytes(pattern, optional);
        var results = new WordResults();


        FindWordsUsingLetters(Root, letters, new WordResults.Word(), results, true);
        return results;
    }

    // Recursive helper for both functions
    private void FindWordsUsingLetters(
        LetterNode currentNode,
        List<WordResults.Tile> availableLetters,
        WordResults.Word currentWord,
        WordResults results,
        bool requireExactMatch)
    {
        foreach (var child in currentNode.Children)
        {
            if (child.Letter != TilesUtils.PivotByte)
            {
                if (availableLetters.Any(p => p.Letter == child.Letter) || availableLetters.Any(p => p.IsJoker))
                {
                    bool isJoker = false;

                    var l = availableLetters.FirstOrDefault(p => p.Letter == child.Letter);
                    if (l != null)
                    {
                        currentWord.Tiles.Add(l);
                        availableLetters.Remove(l);
                    }
                    else
                    {
                        l = availableLetters.FirstOrDefault(p => p.IsJoker);
                        if (l != null)
                        {
                            isJoker = true;
                            currentWord.Tiles.Add(new WordResults.Tile(child.Letter, true, l.Status));
                            availableLetters.Remove(l);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (child.IsEnd)
                    {
                        if (!requireExactMatch || availableLetters.Count == 0)
                        {
                            results.Words.Add(new WordResults.Word(currentWord));
                        }
                    }

                    FindWordsUsingLetters(child, availableLetters, currentWord, results, requireExactMatch);

                    // Backtrack to restore statev
                    if (!isJoker)
                    {
                        availableLetters.Add(l);
                    }
                    else
                    {
                        availableLetters.Add(l);
                    }

                    currentWord.Tiles.RemoveAt(currentWord.Tiles.Count - 1);
                    isJoker = false;
                }
            }
        }
    }
}


//public class Program
//{
//    public static void Main()
//    {

//    }
//}
