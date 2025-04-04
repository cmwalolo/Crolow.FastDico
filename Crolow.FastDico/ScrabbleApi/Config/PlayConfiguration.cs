using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.ScrabbleApi.Config
{
    public partial class PlayConfiguration
    {
        public BagConfiguration BagConfig { get; set; }
        public GridConfigurationContainer GridConfig { get; set; }

        public GameConfig SelectedConfig { get; set; }

    }
}
