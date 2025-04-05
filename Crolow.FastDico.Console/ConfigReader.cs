using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.Config;
using Newtonsoft.Json;

namespace Crolow.FastDico.Console
{
    public partial class ConfigReader
    {

        public class BoardData
        {
            public BoardGrid Grid { get; set; }
        }

        public static BagConfiguration ReadLetterConfig(string language)
        {
            var letterConfig = File.ReadAllText($"Letters_{language}.json");
            var letterData = JsonConvert.DeserializeObject<LetterData>(letterConfig);
            var config = new BagConfiguration();

            config.Name = letterData.Name;
            foreach (var letter in letterData.Letters)
            {
                config.LettersByByte.Add(letter.Letter, letter);
                config.LettersByChar.Add(letter.Char, letter);
            }
            return config;
        }

        public PlayConfiguration ReadConfiguration(string configFiles, string configName)
        {
            PlayConfiguration g = new PlayConfiguration();
            var gridConfigs = File.ReadAllText(configFiles);
            var configs = JsonConvert.DeserializeObject<GameConfigContainer>(gridConfigs);
            g.SelectedConfig = configs.Configurations.First(p => p.Name == configName);
            FillGridConfig(g);
            return g;
        }

        public static PlayConfiguration FillGridConfig(PlayConfiguration g)
        {

            g.BagConfig = ReadLetterConfig(g.SelectedConfig.LetterConfigFile);

            var gridConfig = File.ReadAllText($"GridConfig_{g.SelectedConfig.LetterConfigFile}.json");
            var boardData = JsonConvert.DeserializeObject<BoardData>(gridConfig);

            g.GridConfig = new GridConfigurationContainer(boardData.Grid.SizeH, boardData.Grid.SizeV);
            int sizeH = g.GridConfig.SizeH;
            foreach (var multiplierData in boardData.Grid.Configuration)
            {
                foreach (var position in multiplierData.Positions)
                {
                    int row = position[0]; // Adjusting for zero-based index
                    int col = position[1]; // Adjusting for zero-based index

                    if (multiplierData.Multiplier > 0)
                    {
                        g.GridConfig.Grid[row, col].LetterMultiplier = multiplierData.Multiplier;
                    }
                    else
                    {
                        g.GridConfig.Grid[row, col].WordMultiplier = Math.Abs(multiplierData.Multiplier);
                    }
                }
            }

            return g;
        }
    }
}
