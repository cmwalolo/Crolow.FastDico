using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class LetterBag
{
    private List<Tile> Letters;
    private Random RandomGen;
    private PlayConfiguration GameConfig;
    private CurrentGame CurrentGame;
    public LetterBag(CurrentGame currentGame)
    {
        CurrentGame = currentGame;
        GameConfig = currentGame.Configuration;
        Letters = new List<Tile>();
        RandomGen = new Random();

        // Populate the bag according to the distribution
        foreach (var kvp in GameConfig.BagConfig.Letters)
        {
            for (var i = 0; i < kvp.TotalLetters; i++)
            {
                Letters.Add(new Tile(kvp));
            }
        }
    }

    // Draw a specified number of letters from the bag
    public List<Tile> DrawLetters(PlayerRack rack)
    {
        int count = GameConfig.SelectedConfig.InRackLetters;
        var drawnLetters = rack.GetTiles();
        bool ok = true;
        if (IsValid(Letters, drawnLetters))
        {
            do
            {
                if (!ok)
                {
                    ReturnLetters(drawnLetters);
                }

                for (int i = drawnLetters.Count; i < count; i++)
                {
                    if (!IsEmpty)
                    {
                        int index = RandomGen.Next(RemainingLetters);
                        drawnLetters.Add(Letters[index]);
                        Letters.RemoveAt(index);
                        continue;
                    }
                    break;
                }
                ok = IsValid(drawnLetters, null);
            } while (!ok);
        }
        return drawnLetters;

    }

    // Add letters back to the bag
    public void ReturnLetters(List<Tile> letters)
    {
        Letters.AddRange(letters);
        letters.Clear();
    }

    // Get the number of letters remaining in the bag
    public int RemainingLetters => Letters.Count;

    // Check if the bag is empty
    public bool IsEmpty => Letters.Count == 0;

    public bool IsValid(List<Tile> letters, List<Tile> rack)
    {
        int vow = letters.Sum(p => p.IsVowel ? 1 : 0) + (rack?.Sum(p => p.IsVowel ? 1 : 0) ?? 0);
        int con = letters.Sum(p => p.IsConsonant ? 1 : 0) + (rack?.Sum(p => p.IsConsonant ? 1 : 0) ?? 0);

        return vow >= 1 && con >= 1;
    }

    internal List<Tile> ForceDrawLetters(string v)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var c in v)
        {
            int ndx = Letters.FindIndex(p => c == '?' ? p.Letter == 26 : p.Letter == c - 'a');
            tiles.Add(Letters[ndx]);
            Letters.RemoveAt(ndx);
        }

        return tiles;
    }
}
