using Crolow.FastDico.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data.Interfaces;

namespace Crolow.Pix.Core.Services.Storage
{
    public class GameConfigService : IGameConfigService
    {
        public IDataFactory dataFactory;

        public GameConfigService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<GameConfigModel> LoadAll()
        {
            return dataFactory.Games.GetAllNodes().Result.ToList();
        }

        public void Update(GameConfigModel gameConfig)
        {
            dataFactory.Games.Update(gameConfig);
            gameConfig.EditState = EditState.Unchanged;
        }

    }
}
