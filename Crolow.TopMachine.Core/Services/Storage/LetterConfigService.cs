using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Interfaces;

namespace Crolow.Pix.Core.Services.Storage
{
    public class LetterService : ILetterService
    {
        public IDataFactory dataFactory;

        public LetterService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<LetterConfigModel> LoadAll()
        {
            return dataFactory.LetterConfigs.GetAllNodes().Result.ToList();
        }

        public void Update(LetterConfigModel gameConfig)
        {
            dataFactory.LetterConfigs.Update(gameConfig);
            gameConfig.EditState = EditState.Unchanged;
        }

    }
}
