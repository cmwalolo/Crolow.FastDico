using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.Utils;
using static Crolow.FastDico.ScrabbleApi.ScrabbleAI;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class Board
{
    // Current State of the board
    public GridConfigurationContainer[] CurrentBoard = new GridConfigurationContainer[2];

    public Board(CurrentGame currentGame)
    {
        CurrentBoard[0] = new GridConfigurationContainer(currentGame.Configuration.GridConfig);
        CurrentBoard[1] = new GridConfigurationContainer();
        CurrentBoard[1].SizeV = CurrentBoard[1].SizeH;
        CurrentBoard[1].SizeH = CurrentBoard[1].SizeV;
        CurrentBoard[1].Grid = ArrayUtils.Transpose<Square>(CurrentBoard[0].Grid);

    }

    public Square GetSquare(int grid, int x, int y)
    {
        if (x < 0 || x >= CurrentBoard[grid].Grid.GetLength(0) ||
            y < 0 || y >= CurrentBoard[grid].Grid.GetLength(1))
        {
            return null;
        }
        return CurrentBoard[grid].Grid[x, y];
    }

    public void SetTile(int grid, int X, int Y, Tile tile)
    {
        // WE set definetly the tile on the rack
        tile.Status = 1;
        CurrentBoard[grid].Grid[X, Y].CurrentLetter = tile;
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
            SetTile(0, x, y, tile);
            x += incH;
            y += incV;
        }

        CurrentBoard[1].Grid = ArrayUtils.Transpose<Square>(CurrentBoard[0].Grid);
    }
}
