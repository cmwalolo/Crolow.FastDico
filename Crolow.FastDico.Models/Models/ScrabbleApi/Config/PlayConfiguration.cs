using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.ScrabbleApi.Config
{
    public partial class PlayConfiguration
    {
        public BagConfiguration BagConfig { get; set; }
        public GridConfigurationContainer GridConfig { get; set; }

        public IGameConfigModel SelectedConfig { get; set; }

    }
}
