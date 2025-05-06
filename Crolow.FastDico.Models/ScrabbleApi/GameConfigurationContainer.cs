using Crolow.FastDico.Models.Dictionary.Entities;
using Crolow.FastDico.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Models.ScrabbleApi
{

    public class GameConfigurationContainer
    {
        public GameConfigurationContainer(GameConfigModel game, BoardGridModel board, LetterConfigModel letter, DictionaryModel dico)
        {
            GameConfig = game;
            BoardGrid = board;
            LetterConfig = letter;
            Dictionary = dico;
        }

        public GameConfigurationContainer()
        {
        }

        public bool IsValid { get; set; }
        public GameConfigModel GameConfig { get; set; }
        public BoardGridModel BoardGrid { get; set; }
        public LetterConfigModel LetterConfig { get; set; }
        public DictionaryModel Dictionary { get; set; }
    }
}
