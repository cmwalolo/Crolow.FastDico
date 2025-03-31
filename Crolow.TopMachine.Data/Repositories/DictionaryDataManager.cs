using Crolow.TopMachine.Data.Interfaces;
namespace Crolow.TopMachine.Data.Repositories;

public class DictionaryDataManager<T> : DataManager<T> where T : IDataObject
{
    public DictionaryDataManager(DatabaseSettings context) : base(context, "TopMachine", "Dictionaries")
    {
    }
}
