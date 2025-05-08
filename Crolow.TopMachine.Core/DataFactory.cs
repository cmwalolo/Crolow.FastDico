using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.TopMachine.Core
{
    public class DataFactory : IDataFactory
    {
        private DatabaseSettings settings;
        public DataFactory(DatabaseSettings settings)
        {
            this.settings = settings;
        }

        public IDataManager<ILetterConfigModel> LetterConfigs => new LetterConfigDataManager<ILetterConfigModel>(settings);
        public IDataManager<IBoardGridModel> Boards => new BoardConfigDataManager<IBoardGridModel>(settings);
        public IDataManager<IGameConfigModel> Games => new GameConfigDataManager<IGameConfigModel>(settings);
        public IDataManager<IDictionaryModel> Dictionaries => new DictionaryDataManager<IDictionaryModel>(settings);
        public IDataManager<IWordEntryModel> DicoEntries => new DicoWordsDataManager<IWordEntryModel>(settings);
        public IDataManager<IWordToDicoModel> DicoWords => new DicoWordsToDicoDataManager<IWordToDicoModel>(settings);
        public IDataManager<IUser> Users => new UserDataManager<IUser>(settings);
    }
}
