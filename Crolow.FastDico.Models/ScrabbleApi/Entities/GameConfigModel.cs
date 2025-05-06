using Kalow.Apps.Common.DataTypes;
using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Models.ScrabbleApi.Entities
{


    public class GameConfigModel : DataObject
    {
        public GameConfigModel()
        {
            Bonus = new int[] { 0, 0, 0, 0, 0, 0, 50, 75, 100, 125, 150, 175, 200, 225, 250 };
        }
        public string Name { get; set; }
        public KalowId LetterConfig { get; set; }
        public KalowId BoardConfig { get; set; }

        public int[] Bonus { get; }
        public int PlayableLetters { get; set; }
        public int InRackLetters { get; set; }
        public int TimeByTurn { get; set; }
        public bool JokerMode { get; set; }
        public bool ToppingMode { get; set; }
        public bool DifficultMode { get; set; }
        public bool HelpAvailable { get; set; }
        public bool ShowDictionary { get; set; }
        public int CheckDistributionRound { get; set; }
    }
}
