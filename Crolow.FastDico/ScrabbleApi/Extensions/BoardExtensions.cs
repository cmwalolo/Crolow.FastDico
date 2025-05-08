using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.Utils;
namespace Crolow.FastDico.ScrabbleApi.Extensions;

public static class BoardExtensions
{
    public static Square GetSquare(this Board b, int grid, int x, int y)
    {
        if (x < 0 || x >= b.CurrentBoard[grid].Grid.GetLength(0) ||
            y < 0 || y >= b.CurrentBoard[grid].Grid.GetLength(1))
        {
            return null;
        }
        return b.CurrentBoard[grid].Grid[x, y];
    }

    public static void SetTile(this Board b, int grid, int X, int Y, Tile tile, int status)
    {
        // WE set definetly the tile on the rack
        b.CurrentBoard[grid].Grid[X, Y].CurrentLetter = tile;
        b.CurrentBoard[grid].Grid[X, Y].Status = status;
    }

    public static void RemoveTile(this Board b, int grid, int X, int Y)
    {
        // WE set definetly the tile on the rack
        b.CurrentBoard[grid].Grid[X, Y].CurrentLetter = new Tile();
        b.CurrentBoard[grid].Grid[X, Y].Status = -1;
    }

    public static void SetRound(this Board b, PlayableSolution round)
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
            if (b.currentGame.Configuration.SelectedConfig.JokerMode && tile.IsJoker)
            {
                newTile = b.currentGame.LetterBag.ReplaceJoker(tile);
            }
            b.SetTile(0, x, y, newTile, 1);
            x += incH;
            y += incV;
        }
        Console.WriteLine("-------------------------------------------");

        b.TransposeGrid();
    }

    public static void TransposeGrid(this Board b)
    {
        b.CurrentBoard[1].Grid = ArrayUtils.Transpose(b.CurrentBoard[0].Grid);

    }
}
