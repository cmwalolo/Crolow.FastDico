using Crolow.FastDico.Common.Interfaces;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.Pix.Core.Services.Storage
{
    public class GameConfigService : IGameConfigService
    {
        public IDataFactory dataFactory;

        public GameConfigService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<IGameConfigModel> LoadAll()
        {
            return dataFactory.Games.GetAllNodes().Result.ToList();
        }

        public void Update(IGameConfigModel gameConfig)
        {
            dataFactory.Games.Update(gameConfig);
            gameConfig.EditState = EditState.Unchanged;
        }

    }
}
