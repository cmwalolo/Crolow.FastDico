using Crolow.Fast.Dawg.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class Board
{
    // Current State of the board
    public GridConfigurationContainer CurrentBoard;

    public Board(GameConfiguration config)
    {
        CurrentBoard = new GridConfigurationContainer(config.GridConfig);
    }

    public Square GetTile(Position p) => CurrentBoard.Grid[p.X, p.Y];
    public Square GetTile(int X, int Y) => CurrentBoard.Grid[X, Y];

    public void SetTile(Position p, Tile tile)
    {
        CurrentBoard.Grid[p.X, p.Y].CurrentLetter = new Tile(tile);
    }
}
