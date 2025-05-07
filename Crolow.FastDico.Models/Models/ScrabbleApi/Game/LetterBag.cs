using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.FastDico.Common.Models.ScrabbleApi.Game;

public class LetterBag
{
    public List<Tile> Letters { get; set; }
    public PlayConfiguration GameConfig { get; set; }
    public CurrentGame CurrentGame { get; set; }

    public int RemainingLetters => Letters.Count;
    public bool IsEmpty => Letters.Count == 0;

    public LetterBag(CurrentGame currentGame)
    {
        CurrentGame = currentGame;
        GameConfig = currentGame.Configuration;
        Letters = new List<Tile>();

        // Populate the bag according to the distribution
        foreach (var kvp in GameConfig.BagConfig.LettersByByte)
        {
            for (var i = 0; i < kvp.Value.TotalLetters; i++)
            {
                Letters.Add(new Tile(kvp.Value, null));
            }
        }
    }

    public LetterBag(LetterBag bag)
    {
        Letters = new List<Tile>();
        CurrentGame = bag.CurrentGame;
        GameConfig = bag.GameConfig;
        Letters.AddRange(bag.Letters);
    }
}
