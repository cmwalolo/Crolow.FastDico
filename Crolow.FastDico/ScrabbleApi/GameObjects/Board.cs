using Crolow.FastDico.ScrabbleApi.Config;
using static Crolow.FastDico.ScrabbleApi.ScrabbleAI;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class Board
{
    // Current State of the board
    public GridConfigurationContainer CurrentBoard;

    public Board(CurrentGame currentGame)
    {
        CurrentBoard = new GridConfigurationContainer(currentGame.Configuration.GridConfig);
    }

    public Square GetSquare(Position p) => CurrentBoard.Grid[p.X, p.Y];
    public Square GetSquare(int X, int Y)
    {
        if (X < 0 || X >= CurrentBoard.Grid.GetLength(0) ||
            Y < 0 || Y >= CurrentBoard.Grid.GetLength(1))
        {
            return null;
        }
        return CurrentBoard.Grid[X, Y];
    }

    public void SetTile(int X, int Y, Tile tile)
    {
        // WE set definetly the tile on the rack
        tile.Status = 1;
        CurrentBoard.Grid[X, Y].CurrentLetter = tile;
    }

    public void SetRound(PlayedRound round)
    {
        int incH = 0;
        int incV = 0;
        int x = round.Position.X;
        int y = round.Position.Y;

        if (round.Position.Direction == 0)
        {
            incH = 1;
        }
        else
        {
            incV = 1;
        }

        foreach (var tile in round.Tiles)
        {
            SetTile(x, y, tile);
            tile.Status = 1;
            x += incH;
            y += incV;
        }
    }
}
