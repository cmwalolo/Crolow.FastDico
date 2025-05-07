namespace Crolow.FastDico.Common.Models.ScrabbleApi;

public class Player
{
    public string Name { get; }
    public PlayerRack Rack { get; set; } // List of letters (bytes) on the player's rack
    public Player(string name)
    {
        Name = name;
    }
}
