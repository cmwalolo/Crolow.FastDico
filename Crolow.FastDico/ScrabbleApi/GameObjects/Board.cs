using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.Utils;
namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class Board
{
    // Current State of the board
    public GridConfigurationContainer[] CurrentBoard = new GridConfigurationContainer[2];
    private CurrentGame currentGame;

    public Board(CurrentGame currentGame)
    {
        CurrentBoard[0] = new GridConfigurationContainer(currentGame.Configuration.GridConfig);
        CurrentBoard[1] = new GridConfigurationContainer();
        CurrentBoard[1].SizeV = CurrentBoard[0].SizeH;
        CurrentBoard[1].SizeH = CurrentBoard[0].SizeV;
        CurrentBoard[1].Grid = ArrayUtils.Transpose<Square>(CurrentBoard[0].Grid);
        this.currentGame = currentGame;
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

    public void SetTile(int grid, int X, int Y, Tile tile, int status)
    {
        // WE set definetly the tile on the rack
        CurrentBoard[grid].Grid[X, Y].CurrentLetter = tile;
        CurrentBoard[grid].Grid[X, Y].Status = status;
    }

    public void RemoveTile(int grid, int X, int Y)
    {
        // WE set definetly the tile on the rack
        CurrentBoard[grid].Grid[X, Y].CurrentLetter = new Tile();
        CurrentBoard[grid].Grid[X, Y].Status = -1;
    }

    public void SetRound(PlayableSolution round)
    {
        int incH = 0;
        int incV = 0;
        int x = round.Position.X;
        int y = round.Position.Y;

        round.DebugRound("Word");

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
            var newTile = tile;
            if (currentGame.Configuration.SelectedConfig.JokerMode && tile.IsJoker)
            {
                newTile = currentGame.LetterBag.ReplaceJoker(tile);
            }
            SetTile(0, x, y, newTile, 1);
            x += incH;
            y += incV;
        }
        Console.WriteLine("-------------------------------------------");

        TransposeGrid();
    }

    public void TransposeGrid()
    {
        CurrentBoard[1].Grid = ArrayUtils.Transpose<Square>(CurrentBoard[0].Grid);

    }
}
