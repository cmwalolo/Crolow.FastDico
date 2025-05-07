using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi.Extensions
{
    public static class SquareExtensions
    {
        public static void SetPivot(this Square sq, byte letter, int direction, int points)
        {
            sq.Pivots[direction] = sq.Pivots[direction] | 1u << letter;
            sq.PivotPoints[direction] = points;
        }

        public static void SetPivot(this Square sq, uint letter, int direction, int points)
        {
            sq.Pivots[direction] = letter;
            sq.PivotPoints[direction] = points;
        }

        public static void SetPivotLetters(this Square sq, int letters, int direction)
        {
            sq.PivotLetters[direction] = letters;

        }

        public static bool GetPivot(this Square sq, Tile letter, int direction, byte joker)
        {
            var c = letter.IsJoker ? joker : letter.Letter;
            return (sq.Pivots[direction] & 1u << c) > 0;
        }

        public static uint GetPivot(this Square sq, int direction)
        {
            return sq.Pivots[direction];
        }

        public static int GetPivotLetters(this Square sq, int direction)
        {
            return sq.PivotLetters[direction];
        }

        public static int GetPivotPoints(this Square sq, int direction)
        {
            return sq.PivotPoints[direction];
        }


        public static void ResetPivot(this Square sq, int grid, int points, uint maskValue = uint.MaxValue)
        {
            sq.Pivots[grid] = maskValue;
            sq.PivotPoints[grid] = points;
            sq.PivotLetters[grid] = 0;
        }


        public static int GetPivotPoint(this Square sq, byte letter, int direction)
        {
            return sq.PivotPoints[direction];
        }

    }
}
