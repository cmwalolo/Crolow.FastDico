namespace Kalow.Hypergram.Logic.Models.GamePlay.Containers
{
    public class HypergramBoardConfigContainer
    {
        public int[] Tiles_vowels { get; set; }
        public int[] Tiles_consonants { get; set; }
        public int[] Tiles_numbers { get; set; }
        public int[] Tiles_points { get; set; }
        public char[] Tiles_ascii { get; set; }
        public int TotalTiles { get; set; }
        public int JokerTitle { get; set; }
        public int NbLetters { get; set; }

    }
}
