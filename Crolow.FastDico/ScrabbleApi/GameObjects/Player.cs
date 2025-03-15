namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class Player
{
    public string Name { get; }
    public PlayerRack Rack { get; set; } // List of letters (bytes) on the player's rack

    public Player(string name)
    {
        Name = name;
    }

    // Draw a number of letters from the LetterBag
    public void SetRack(Tile[] Letters)
    {
        Rack.Tiles.AddRange(Letters);
    }

    // Play a word (remove letters from the rack)
    public bool PlayWord(List<Tile> word)
    {
        return true;
    }
}
