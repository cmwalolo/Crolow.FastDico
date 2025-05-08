using Crolow.TopMachine.Data.Bridge;

namespace Crolow.TopMachine.Data.Repositories;

public class DicoWordsDataManager<T> : DataManager<T> where T : IDataObject
{
    public DicoWordsDataManager(DatabaseSettings context) : base(context, "Dictionary", "Words")
    {
    }
}
