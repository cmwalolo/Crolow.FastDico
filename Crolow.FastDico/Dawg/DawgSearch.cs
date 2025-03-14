using Crolow.FastDico.Dicos;
using Crolow.FastDico.Interfaces;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.Dawg;

public class DawgSearch : IDawgSearch
{
    public LetterNode Root { get; private set; }
    public DawgSearch(LetterNode root)
    {
        Root = root;
    }

    #region Search functions
    // Search for a specific word
    public bool SearchWord(string word)
    {
        var currentNode = Root;
        List<byte> byteWord = DawgUtils.ConvertWordToBytes(word);

        foreach (var byteVal in byteWord)
        {
            var target = currentNode.Children.Where(p => p.Letter == byteVal);
            if (!target.Any())
            {
                return false; // Word not found
            }
            currentNode = target.First();
        }

        return currentNode.IsEnd; // Return true if the node is terminal
    }

    // Search for all words beginning with a given prefix
    public List<string> SearchByPrefix(string prefix)
    {
        var currentNode = Root;
        List<byte> bytePrefix = DawgUtils.ConvertWordToBytes(prefix);
        List<string> results = new List<string>();

        // Traverse to the node that represents the end of the prefix
        foreach (var byteVal in bytePrefix)
        {
            var target = currentNode.Children.Where(p => p.Letter == byteVal);
            if (!target.Any())
            {
                return results; // No words found with this prefix
            }
            currentNode = target.First();
        }

        // Once we reach the prefix node, gather all words that start with this prefix
        FindAllWordsFromNode(currentNode, bytePrefix, results);
        return results;
    }

    public List<string> SearchBySuffix(string prefix)
    {
        return SearchByPattern("*" + prefix);
    }

    // Helper method to find all words from a given node
    private void FindAllWordsFromNode(LetterNode node, List<byte> prefix, List<string> results)
    {
        if (node.IsEnd)
        {
            results.Add(DawgUtils.ConvertBytesToWord(prefix));
        }

        foreach (var child in node.Children)
        {
            prefix.Add(child.Letter);
            FindAllWordsFromNode(child, prefix, results);
            prefix.RemoveAt(prefix.Count - 1); // backtrack
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
                currentWord.Add(child.Letter);
                SearchByPatternRecursive(child, bytePattern, patternIndex, currentWord, results);
                currentWord.RemoveAt(currentWord.Count - 1); // Backtrack
            }
        }
        else if (currentByte == 30) // '?' wildcard
        {
            // Match exactly one character
            foreach (var child in currentNode.Children)
            {
                currentWord.Add(child.Letter);
                SearchByPatternRecursive(child, bytePattern, patternIndex + 1, currentWord, results);
                currentWord.RemoveAt(currentWord.Count - 1); // Backtrack
            }
        }
        else
        {
            // Match the exact character
            var nextNode = currentNode.Children.Where(p => p.Letter == currentByte);
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
    #endregion
}


