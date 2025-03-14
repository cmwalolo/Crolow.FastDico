using Crolow.FastDico.Dicos;
using Crolow.FastDico.Interfaces;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.GadDag;

public class GadDagSearch : IDawgSearch
{
    public LetterNode Root { get; private set; }

    public GadDagSearch(LetterNode root)
    {
        Root = root;
    }

    public bool SearchWord(string word)
    {
        var bytes = DawgUtils.ConvertWordToBytes(word);
        return SearchWordRecursive(Root, bytes, 0, false);
    }

    private bool SearchWordRecursive(LetterNode currentNode, List<byte> word, int index, bool pastPivot)
    {
        if (index == word.Count)
        {
            return currentNode.IsEnd;
        }

        byte currentByte = word[index];

        // Traverse children to find the matching letter
        foreach (var child in currentNode.Children)
        {
            if (child.Letter == currentByte || !pastPivot && child.Letter == DawgUtils.PivotByte) // Pivot handling
            {
                if (SearchWordRecursive(child, word, index + 1, pastPivot || child.Letter == DawgUtils.PivotByte))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public List<string> SearchByPrefix(string prefix)
    {
        var bytes = DawgUtils.ConvertWordToBytes(prefix);
        var results = new List<string>();
        var currentNode = Root;

        // Navigate to the prefix node
        foreach (var byteVal in bytes)
        {
            currentNode = currentNode.Children.FirstOrDefault(c => c.Letter == byteVal);
            if (currentNode == null)
                return results; // Prefix not found
        }

        // Collect all words from the prefix node
        SearchPrefixesFromNode(currentNode, bytes, results);
        return results;
    }

    private void SearchPrefixesFromNode(LetterNode node, List<byte> currentWord, List<string> results)
    {
        if (node.IsEnd)
        {
            results.Add(DawgUtils.ConvertBytesToWord(currentWord));
        }

        foreach (var child in node.Children)
        {
            if (child.Letter != DawgUtils.PivotByte)
            {
                currentWord.Add(child.Letter);
                SearchPrefixesFromNode(child, currentWord, results);
                currentWord.RemoveAt(currentWord.Count - 1); // Backtrack
            }
        }
    }


    public List<string> SearchBySuffix(string suffix)
    {
        var patternedSuffix = suffix + "#";

        var bytes = DawgUtils.ConvertWordToBytes(patternedSuffix);
        var results = new List<string>();
        var currentNode = Root;

        // Navigate to the prefix node
        foreach (var byteVal in bytes)
        {
            currentNode = currentNode.Children.FirstOrDefault(c => c.Letter == byteVal);
            if (currentNode == null)
                return results; // Prefix not found
        }

        // Collect all words from the prefix node
        SearchSuffixesFromNode(currentNode, bytes, new List<byte>(), results);
        return results;
    }

    private void SearchSuffixesFromNode(LetterNode node, List<byte> currentWord, List<byte> result, List<string> results)
    {
        if (node.Children.Count == 0)
        {
            var newWord = new List<byte>();
            var word = currentWord.Take(currentWord.Count - 1).ToList();
            newWord.AddRange(result);
            newWord.AddRange(word);
            results.Add(DawgUtils.ConvertBytesToWord(newWord));
        }

        foreach (var child in node.Children)
        {
            result.Insert(0, child.Letter);
            SearchSuffixesFromNode(child, currentWord, result, results);
            result.RemoveAt(0);
        }
    }

    public List<string> SearchByPattern(string pattern)
    {
        // Convert the pattern into bytes
        List<byte> bytePattern = ConvertPatternToBytes(pattern);
        List<string> results = new List<string>();
        SearchByPatternRecursive(Root, bytePattern, 0, new List<byte>(), results);
        return results;
    }

    private void SearchByPatternRecursive(LetterNode currentNode, List<byte> bytePattern, int patternIndex, List<byte> currentWord, List<string> results)
    {
        // Base case: Reached the end of the pattern
        if (patternIndex == bytePattern.Count)
        {
            if (currentNode.IsEnd)
            {
                results.Add(DawgUtils.ConvertBytesToWord(currentWord));
            }
            return;
        }

        byte currentByte = bytePattern[patternIndex];

        if (currentByte == 31) // '*' wildcard
        {
            // Match zero or more characters
            // First, try skipping the '*'
            SearchByPatternRecursive(currentNode, bytePattern, patternIndex + 1, currentWord, results);

            // Then, try matching one or more characters
            foreach (var child in currentNode.Children)
            {
                if (child.Letter != DawgUtils.PivotByte)
                {
                    currentWord.Add(child.Letter);
                    SearchByPatternRecursive(child, bytePattern, patternIndex, currentWord, results);
                    currentWord.RemoveAt(currentWord.Count - 1); // Backtrack
                }
            }
        }
        else if (currentByte == 30) // '?' wildcard
        {
            // Match exactly one character
            foreach (var child in currentNode.Children)
            {
                if (child.Letter != DawgUtils.PivotByte)
                {
                    currentWord.Add(child.Letter);
                    SearchByPatternRecursive(child, bytePattern, patternIndex + 1, currentWord, results);
                    currentWord.RemoveAt(currentWord.Count - 1); // Backtrack
                }
            }
        }
        else
        {
            // Match the exact character
            var nextNode = currentNode.Children.Where(p => p.Letter != DawgUtils.PivotByte && p.Letter == currentByte);
            if (nextNode.Any())
            {
                currentWord.Add(currentByte);
                SearchByPatternRecursive(nextNode.First(), bytePattern, patternIndex + 1, currentWord, results);
                currentWord.RemoveAt(currentWord.Count - 1); // Backtrack
            }
        }
    }

    private List<byte> ConvertPatternToBytes(string pattern)
    {
        List<byte> bytePattern = new List<byte>();
        foreach (char c in pattern)
        {
            if (c == '*')
                bytePattern.Add(31); // '*' wildcard
            else if (c == '?')
                bytePattern.Add(30); // '?' wildcard
            else
                bytePattern.Add((byte)(c - 'a'));
        }
        return bytePattern;
    }

    // Function 1: Find all words that can be formed using exactly the given letters
    public List<string> FindAllWordsFromLetters(string pattern)
    {
        var letters = DawgUtils.ConvertWordToBytes(pattern);
        var results = new List<string>();
        FindWordsUsingLetters(Root, letters, new List<byte>(), results, true);
        return results;
    }

    // Function 2: Find all words that contain at least one of the given letters
    public List<string> FindAllWordsContainingLetters(string pattern)
    {
        var letters = DawgUtils.ConvertWordToBytes(pattern);
        var results = new List<string>();
        FindWordsUsingLetters(Root, letters, new List<byte>(), results, false);
        return results;
    }

    // Recursive helper for both functions
    private void FindWordsUsingLetters(
        LetterNode currentNode,
        List<byte> availableLetters,
        List<byte> currentWord,
        List<string> results,
        bool requireExactMatch)
    {
        foreach (var child in currentNode.Children)
        {
            if (availableLetters.Contains(child.Letter))
            {
                // Use the letter, removing it from available letters
                currentWord.Add(child.Letter);
                availableLetters.Remove(child.Letter);

                if (child.IsEnd)
                {
                    if (!requireExactMatch || availableLetters.Count == 0)
                    {
                        results.Add(DawgUtils.ConvertBytesToWord(currentWord));
                    }
                }

                FindWordsUsingLetters(child, availableLetters, currentWord, results, requireExactMatch);

                // Backtrack to restore statev
                availableLetters.Add(child.Letter);
                currentWord.RemoveAt(currentWord.Count - 1);
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
