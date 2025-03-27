namespace Crolow.FastDico.ScrabbleApi.GameObjects
{
    public class Square
    {
        public int LetterMultiplier { get; set; } = 1;
        public int WordMultiplier { get; set; } = 1;
        public bool IsBorder { get; set; } = true;
        public Tile CurrentLetter { get; set; }

        public uint[] Pivots { get; set; } = new uint[2] { uint.MaxValue, uint.MaxValue };
        public int[] PivotPoints { get; set; } = new int[2];

        public void SetPivot(byte letter, int direction, int points)
        {
            Pivots[direction] = Pivots[direction] | 1u << letter;
            PivotPoints[direction] = points;
        }

        public bool GetPivot(byte letter, int direction)
        {
            return (Pivots[direction] & 1u << letter) > 0;
        }


        public void ResetPivot(int grid, uint maskValue = uint.MaxValue)
        {
            Pivots[grid] = maskValue;
            PivotPoints[grid] = 0;
        }


        public int GetPivotPoint(byte letter, int direction)
        {
            return PivotPoints[direction];
        }

    }
}
