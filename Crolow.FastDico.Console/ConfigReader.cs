using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Newtonsoft.Json;

namespace Crolow.FastDico.Console
{
    public class ConfigReader
    {
        public List<IDictionaryModel> Dictionaries { get; set; }
        public List<ILetterConfigModel> Letters { get; set; }
        public List<IBoardGridModel> Boards { get; set; }
        public List<IGameConfigModel> GameConfigs { get; set; }

        public class BoardData
        {
            public IBoardGridModel Grid { get; set; }
        }

        public ToppingConfigurationContainer LoadConfig(string name)
        {
            var json = File.ReadAllText($"LetterConfigurations.json");

            var l = JsonConvert.DeserializeObject<List<LetterConfigModel>>(json);
            Letters = l.Select(p => p as ILetterConfigModel).ToList();

            json = File.ReadAllText($"GameConfigurations.json");
            var g = JsonConvert.DeserializeObject<List<GameConfigModel>>(json);
            GameConfigs = g.Select(p => p as IGameConfigModel).ToList();

            json = File.ReadAllText($"BoardConfigurations.json");
            var b = JsonConvert.DeserializeObject<List<BoardGridModel>>(json);
            Boards = b.Select(p => p as IBoardGridModel).ToList();

            json = File.ReadAllText($"Dictionaries.json");
            var d = JsonConvert.DeserializeObject<List<DictionaryModel>>(json);
            Dictionaries = d.Select(p => p as IDictionaryModel).ToList();

            var game = GameConfigs.FirstOrDefault(x => x.Name == name);
            var board = Boards.FirstOrDefault(x => x.Id == game.BoardConfig);
            var letter = Letters.FirstOrDefault(x => x.Id == game.LetterConfig);
            var dico = Dictionaries.FirstOrDefault(x => x.Id == letter.DictionaryId);

            ToppingConfigurationContainer container = new ToppingConfigurationContainer(game, board, letter, dico);
            return container;
        }
    }
}
