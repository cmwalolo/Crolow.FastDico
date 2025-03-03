namespace Crolow.FastDico.ScrabbleApi.Config
{
    public class PlayConfigContainer
    {
        public List<PlayConfig> Configurations { get; set; }
    }
    public class PlayConfig
    {
        public PlayConfig()
        {
            Bonus = new int[] { 0, 0, 0, 0, 0, 0, 50, 75, 100, 125, 150 };
        }

        public int[] Bonus { get; }
        public string Dictionary { get; set; }
        public int GameType { get; set; }
        public int GridType { get; set; }
        public int PlayableLetters { get; set; }
        public int InRackLetters { get; set; }
        public int TimeByTurn { get; set; }
        public int DifficultRatio { get; set; }
        public bool JokerMode { get; set; }
        public bool ToppingMode { get; set; }
        public bool DifficultMode { get; set; }
    }
}
