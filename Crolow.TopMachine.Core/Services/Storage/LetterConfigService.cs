using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.Pix.Core.Services.Storage
{
    public class LetterService : ILetterService
    {
        public IDataFactory dataFactory;

        public LetterService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<ILetterConfigModel> LoadAll()
        {
            return dataFactory.LetterConfigs.GetAllNodes().Result.ToList();
        }

        public void Update(ILetterConfigModel gameConfig)
        {
            dataFactory.LetterConfigs.Update(gameConfig);
            gameConfig.EditState = EditState.Unchanged;
        }

    }
}
