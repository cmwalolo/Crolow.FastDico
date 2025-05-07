using Crolow.FastDico.Common.Models.Common.Entities;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.FastDico.Common.Interfaces;
public interface IDataFactory
{
    LetterConfigDataManager<LetterConfigModel> LetterConfigs { get; }
    BoardConfigDataManager<BoardGridModel> Boards { get; }
    GameConfigDataManager<GameConfigModel> Games { get; }
    DictionaryDataManager<DictionaryModel> Dictionaries { get; }
    DicoWordsDataManager<WordEntryModel> DicoEntries { get; }
    DicoWordsToDicoDataManager<WordToDicoModel> DicoWords { get; }

    UserDataManager<User> Users { get; }

}