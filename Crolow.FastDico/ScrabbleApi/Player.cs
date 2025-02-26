namespace Crolow.Fast.Dawg.ScrabbleApi;

public class Player
{
    public string Name { get; }
    public List<byte> Rack { get; private set; } // List of letters (bytes) on the player's rack
    private LetterBag letterBag;

    public Player(string name, LetterBag letterBag)
    {
        Name = name;
        Rack = new List<byte>();
        this.letterBag = letterBag;
    }

    // Draw a number of letters from the LetterBag
    public void DrawLetters(int count)
    {
        var drawnLetters = letterBag.DrawLetters(count);
        Rack.AddRange(drawnLetters);
    }

    // Play a word (remove letters from the rack)
    public bool PlayWord(List<byte> word)
    {
        foreach (var letter in word)
        {
            if (!Rack.Contains(letter))
            {
                return false; // Cannot play the word if it doesn't fit in the rack
            }
        }

        foreach (var letter in word)
        {
            Rack.Remove(letter); // Remove the letters used for the word from the rack
        }

        return true;
    }
}
