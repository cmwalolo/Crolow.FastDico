using Crolow.TopMachine.Data.Bridge.Entities;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.TopMachine.Data.Bridge;
public interface IDataFactory
{
    IDataManager<ILetterConfigModel> LetterConfigs { get; }
    IDataManager<IBoardGridModel> Boards { get; }
    IDataManager<IGameConfigModel> Games { get; }
    IDataManager<IDictionaryModel> Dictionaries { get; }
    IDataManager<IWordEntryModel> DicoEntries { get; }
    IDataManager<IWordToDicoModel> DicoWords { get; }

    IDataManager<IUser> Users { get; }

}