using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Models.Models.ScrabbleApi.Entities
{
    public class GameConfig : DataObject
    {
        public GameConfig()
        {
            Bonus = new int[] { 0, 0, 0, 0, 0, 0, 50, 75, 100, 125, 150 };
        }

        public string Name { get; set; }
        public string GaddagFile { get; set; }

        public int[] Bonus { get; }
        public string GridConfigFile { get; set; }
        public int GameType { get; set; }
        public int GridType { get; set; }
        public int PlayableLetters { get; set; }
        public int InRackLetters { get; set; }
        public int TimeByTurn { get; set; }
        public int DifficultRatio { get; set; }
        public bool JokerMode { get; set; }
        public bool ToppingMode { get; set; }
        public bool DifficultMode { get; set; }

        public int CheckDistributionRound { get; set; }
    }
}
