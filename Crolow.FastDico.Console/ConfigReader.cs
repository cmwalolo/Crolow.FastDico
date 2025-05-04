using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.Models.Models.ScrabbleApi;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Newtonsoft.Json;

namespace Crolow.FastDico.Console
{
    public class ConfigReader
    {
        public List<DictionaryModel> Dictionaries { get; set; }
        public List<LetterConfigModel> Letters { get; set; }
        public List<BoardGridModel> Boards { get; set; }
        public List<GameConfigModel> GameConfigs { get; set; }

        public class BoardData
        {
            public BoardGridModel Grid { get; set; }
        }

        public GameConfigurationContainer LoadConfig(string name)
        {
            var json = File.ReadAllText($"LetterConfigurations.json");
            Letters = JsonConvert.DeserializeObject<List<LetterConfigModel>>(json);

            json = File.ReadAllText($"GameConfigurations.json");
            GameConfigs = JsonConvert.DeserializeObject<List<GameConfigModel>>(json);

            json = File.ReadAllText($"BoardConfigurations.json");
            Boards = JsonConvert.DeserializeObject<List<BoardGridModel>>(json);

            json = File.ReadAllText($"Dictionaries.json");
            Dictionaries = JsonConvert.DeserializeObject<List<DictionaryModel>>(json);

            var game = GameConfigs.FirstOrDefault(x => x.Name == name);
            var board = Boards.FirstOrDefault(x => x.Id == game.BoardConfig);
            var letter = Letters.FirstOrDefault(x => x.Id == game.LetterConfig);
            var dico = Dictionaries.FirstOrDefault(x => x.Id == letter.DictionaryId);

            GameConfigurationContainer container = new GameConfigurationContainer(game, board, letter, dico);
            return container;
        }
    }
}
