using Crolow.FastDico.ScrabbleApi.Config;
using Newtonsoft.Json;

namespace Crolow.FastDico.ScrabbleApi.Utils
{
    public class ConfigReader
    {
        private class MultiplierData
        {
            public int Multiplier { get; set; }
            public List<int[]> Positions { get; set; }
        }

        private class BoardGrid
        {
            public int SizeH { get; set; }
            public int SizeV { get; set; }
            public List<MultiplierData> Configuration { get; set; }
        }

        private class BoardData
        {
            public PlayConfigContainer PlayConfig { get; set; }
            public BagConfigurationContainer BagConfiguration { get; set; }
            public BoardGrid Grid { get; set; }
        }

        public static GameConfiguration FillGridConfig(string jsonData)
        {
            // Deserialize JSON data into BoardData
            var boardData = JsonConvert.DeserializeObject<BoardData>(jsonData);

            GameConfiguration config = new GameConfiguration();


            config.GridConfig = new GridConfigurationContainer(boardData.Grid.SizeH, boardData.Grid.SizeV);
            config.BagConfig = boardData.BagConfiguration;
            config.PlayConfig = boardData.PlayConfig;

            foreach (var multiplierData in boardData.Grid.Configuration)
            {
                foreach (var position in multiplierData.Positions)
                {
                    int row = position[0]; // Adjusting for zero-based index
                    int col = position[1]; // Adjusting for zero-based index

                    if (multiplierData.Multiplier > 0)
                    {
                        config.GridConfig.Grid[row, col].LetterMultiplier = multiplierData.Multiplier;
                    }
                    else
                    {
                        config.GridConfig.Grid[row, col].WordMultiplier = Math.Abs(multiplierData.Multiplier);
                    }
                }
            }

            return config;
        }
    }
}
