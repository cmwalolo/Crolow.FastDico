namespace Crolow.FastDico.ScrabbleApi.Config
{
    public class Square
    {
        public int LetterMultiplier { get; set; } = 1;
        public int WordMultiplier { get; set; } = 1;
        public bool IsBorder { get; set; } = true;
        public Tile CurrentLetter { get; set; }

        public uint PivotHorizontal { get; set; } = uint.MaxValue;
        public uint PivotVertical { get; set; } = uint.MaxValue;
        public int PivotPointsHorizontal { get; set; }
        public int PivotPointsVertical { get; set; }

        public void SetPivot(byte letter, int direction)
        {
            if (direction == 0)
            {
                PivotHorizontal = PivotHorizontal | (1u << letter);
            }
            else
            {
                PivotVertical = PivotVertical | (1u << letter);
            }
        }

        public bool GetPivot(byte letter, int direction)
        {
            if (direction == 0)
            {
                return (PivotHorizontal & (1u << letter)) > 0;
            }
            else
            {
                return (PivotVertical & (1u << letter)) > 0;
            }
        }

        public void ResetPivot()
        {
            PivotHorizontal = PivotVertical = uint.MaxValue;
        }


    }
}
