namespace Crolow.FastDico.ScrabbleApi.GameObjects
{
    public class Square
    {
        public int LetterMultiplier { get; set; } = 1;
        public int WordMultiplier { get; set; } = 1;
        public bool IsBorder { get; set; } = true;
        public Tile CurrentLetter { get; set; }

        public int Status { get; set; } = -1;
        public uint[] Pivots { get; set; } = new uint[2] { uint.MaxValue, uint.MaxValue };
        public int[] PivotPoints { get; set; } = new int[2];

        public void SetPivot(byte letter, int direction, int points)
        {
            Pivots[direction] = Pivots[direction] | 1u << letter;
            PivotPoints[direction] = points;
        }

        public void SetPivot(uint letter, int direction, int points)
        {
            Pivots[direction] = letter;
            PivotPoints[direction] = points;
        }

        public bool GetPivot(Tile letter, int direction, byte joker)
        {
            var c = letter.IsJoker ? joker : letter.Letter;
            return (Pivots[direction] & 1u << c) > 0;
        }

        public uint GetPivot(int direction)
        {
            return Pivots[direction];
        }

        public int GetPivotPoints(int direction)
        {
            return PivotPoints[direction];
        }


        public void ResetPivot(int grid, int points, uint maskValue = uint.MaxValue)
        {
            Pivots[grid] = maskValue;
            PivotPoints[grid] = points;
        }


        public int GetPivotPoint(byte letter, int direction)
        {
            return PivotPoints[direction];
        }

    }
}
