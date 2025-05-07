using Crolow.FastDico.Common.Models.ScrabbleApi;

namespace Crolow.FastDico.ScrabbleApi.Extensions;

public static class PositionExtensions
{
    public static bool ISGreater(this Position p1, Position p2)
    {
        return p1.X > p2.X || p1.Y > p2.Y;
    }
}
