namespace Crolow.Fast.Dawg.ScrabbleApi;

public class LetterBag
{
    private List<byte> Letters;
    private Random RandomGen;

    public LetterBag(Dictionary<byte, int> distribution)
    {
        Letters = new List<byte>();
        RandomGen = new Random();

        // Populate the bag according to the distribution
        foreach (var kvp in distribution)
        {
            Letters.AddRange(Enumerable.Repeat(kvp.Key, kvp.Value));
        }
    }

    // Draw a specified number of letters from the bag
    public List<byte> DrawLetters(int count)
    {
        if (count > Letters.Count)
            throw new InvalidOperationException("Not enough letters in the bag to draw.");

        var drawnLetters = new List<byte>();
        for (int i = 0; i < count; i++)
        {
            int index = RandomGen.Next(Letters.Count);
            drawnLetters.Add(Letters[index]);
            Letters.RemoveAt(index);
        }
        return drawnLetters;
    }

    // Add letters back to the bag
    public void ReturnLetters(IEnumerable<byte> letters)
    {
        Letters.AddRange(letters);
    }

    // Get the number of letters remaining in the bag
    public int RemainingLetters => Letters.Count;

    // Check if the bag is empty
    public bool IsEmpty => Letters.Count == 0;

    // Debugging: Print the distribution of remaining letters
    public Dictionary<byte, int> GetRemainingDistribution()
    {
        return Letters.GroupBy(letter => letter).ToDictionary(g => g.Key, g => g.Count());
    }
}
