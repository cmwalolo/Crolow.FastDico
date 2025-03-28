using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.Config;

using Newtonsoft.Json;

namespace Crolow.FastDico.ScrabbleApi.Utils
{
    public partial class ConfigReader
    {

        private class BoardData
        {
            public BagConfigurationContainer BagConfiguration { get; set; }
            public BoardGrid Grid { get; set; }
        }

        public PlayConfiguration ReadConfiguration(string configFiles, string configName)
        {
            PlayConfiguration g = new PlayConfiguration();
            var gridConfigs = System.IO.File.ReadAllText(configFiles);
            var configs = JsonConvert.DeserializeObject<GameConfigContainer>(gridConfigs);
            g.SelectedConfig = configs.Configurations.First(p => p.Name == configName);
            FillGridConfig(g);
            return g;
        }

        public static PlayConfiguration FillGridConfig(PlayConfiguration g)
        {
            var gridConfig = System.IO.File.ReadAllText(g.SelectedConfig.GridConfigFile);

            // Deserialize JSON data into BoardData
            var boardData = JsonConvert.DeserializeObject<BoardData>(gridConfig);

            g.GridConfig = new GridConfigurationContainer(boardData.Grid.SizeH, boardData.Grid.SizeV);
            g.BagConfig = boardData.BagConfiguration;

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
