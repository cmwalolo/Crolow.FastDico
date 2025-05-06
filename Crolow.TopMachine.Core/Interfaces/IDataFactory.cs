using Crolow.FastDico.Models.Common.Entities;
using Crolow.FastDico.Models.Dictionary.Entities;
using Crolow.FastDico.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.TopMachine.Core.Interfaces;
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