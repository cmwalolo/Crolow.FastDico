using Crolow.Fast.Dawg.Dawg;
using Crolow.Fast.Dawg.Dicos;

namespace Crolow.Fast.Dawg.ScrabbleApi;

/// <summary>
/// It's just a starting point
/// Still need implementation
/// Thinking of using gaddag and dawg together :
/// - Gaddag to search from RTL and LTR at the first pivot position
/// - Dawg to search from the first pivot position only LTR
/// </summary>
public class ScrabbleAI
{
    private Board board;
    private LetterBag letterBag;
    private DawgSearch gaddag;
    private List<byte> rack;

    public ScrabbleAI(Board board, LetterBag letterBag, DawgSearch gaddag)
    {
        this.board = board;
        this.letterBag = letterBag;
        this.gaddag = gaddag;
    }

    /// <summary>
    ///  All code here is just bullshit 
    /// </summary>
    /// <param name="rack"></param>
    public void FindBestMoveFirstRound(List<byte> rack)
    {
        this.rack = rack;
        int bestScore = 0;
        string bestWord = "";
        int bestX = 0;
        int bestY = 0;
        bool bestHorizontal = true;

        // Start at the center (7, 7) and attempt to build valid words in both directions
        foreach (var letter in rack)
        {
            // Try building words from the center using the current letter
            int score = TryBuildWordFromCenter(letter, 7, 7, true); // horizontal direction
            if (score > bestScore)
            {
                bestScore = score;
                bestWord = "word"; // Replace with actual word
                bestX = 7;
                bestY = 7;
                bestHorizontal = true;
            }

            score = TryBuildWordFromCenter(letter, 7, 7, false); // vertical direction
            if (score > bestScore)
            {
                bestScore = score;
                bestWord = "word"; // Replace with actual word
                bestX = 7;
                bestY = 7;
                bestHorizontal = false;
            }
        }

        Console.WriteLine($"Best word: {bestWord} at ({bestX},{bestY}), Horizontal: {bestHorizontal}, Score: {bestScore}");
    }

    private int TryBuildWordFromCenter(byte startingLetter, int x, int y, bool isHorizontal)
    {
        int score = 0;
        List<byte> currentWord = new List<byte> { startingLetter };

        if (isHorizontal)
        {
            // Try extending the word to the right and left from (x, y)
            score += ExploreWord(x, y, currentWord, true); // right direction
            score += ExploreWord(x, y, currentWord, false); // left direction
        }
        else
        {
            // Try extending the word upwards and downwards from (x, y)
            score += ExploreWord(x, y, currentWord, true); // down direction
            score += ExploreWord(x, y, currentWord, false); // up direction
        }

        return score;
    }

    private int ExploreWord(int x, int y, List<byte> currentWord, bool direction)
    {
        int score = 0;
        LetterNode currentNode = gaddag.Root;

        // Depending on direction, traverse the GADDAG or DAWG
        foreach (byte letter in currentWord)
        {
            // If direction is true, we move to the right or down, else we move to left or up
            var childNode = currentNode.Children.FirstOrDefault(p => p.Letter == letter);
            if (childNode != null)
            {
                currentNode = childNode;
                score += CalculateLetterScore(x, y); // Add letter score
            }
            else
            {
                break;
            }
        }

        return score;
    }

    private int CalculateLetterScore(int x, int y)
    {
        // For simplicity, assuming 1 point per letter (can enhance to include multiplier logic)
        return 1;
    }
}