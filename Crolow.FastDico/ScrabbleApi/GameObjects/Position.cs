namespace Crolow.Fast.Dawg.ScrabbleApi;

public class Position
{
    public int Direction { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Position(int x, int y, int direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }
}
