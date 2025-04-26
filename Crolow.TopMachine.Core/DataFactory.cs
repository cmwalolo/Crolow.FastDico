using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data;
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

        public LetterConfigDataManager<LetterConfigModel> LetterConfigs => new LetterConfigDataManager<LetterConfigModel>(settings);
        public BoardConfigDataManager<BoardGridModel> Boards => new BoardConfigDataManager<BoardGridModel>(settings);
        public GameConfigDataManager<GameConfigModel> Games => new GameConfigDataManager<GameConfigModel>(settings);
        public DictionaryDataManager<DictionaryModel> Dictionaries => new DictionaryDataManager<DictionaryModel>(settings);
        public DicoWordsDataManager<WordEntryModel> DicoEntries => new DicoWordsDataManager<WordEntryModel>(settings);
        public DicoWordsToDicoDataManager<WordToDicoModel> DicoWords => new DicoWordsToDicoDataManager<WordToDicoModel>(settings);
    }
}
