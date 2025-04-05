using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.TopMachine.Core.Interfaces;
public interface IDataFactory
{
    BoardConfigDataManager<BoardGrid> Boards { get; }
    GameConfigDataManager<GameConfig> Games { get; }
    DictionaryDataManager<DictionaryModel> Dictionaries { get; }
    DicoWordsDataManager<WordEntryModel> DicoEntries { get; }
    DicoWordsToDicoDataManager<WordToDicoModel> DicoWords { get; }

}