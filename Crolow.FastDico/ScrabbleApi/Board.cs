namespace Crolow.Fast.Dawg.ScrabbleApi;

public class Board
{
    private Tile[,] Grid;
    public int Rows { get; }
    public int Columns { get; }

    public Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new Tile[rows, columns];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                Grid[i, j] = new Tile();
    }

    public Tile GetTile(int x, int y) => Grid[x, y];

    public void SetTile(int x, int y, byte letter, TileMultiplier multiplier = TileMultiplier.None)
    {
        Grid[x, y] = new Tile(multiplier) { Letter = letter };
    }
}
