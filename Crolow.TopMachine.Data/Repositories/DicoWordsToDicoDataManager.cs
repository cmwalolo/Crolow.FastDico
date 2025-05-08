using Crolow.TopMachine.Data.Bridge;

namespace Crolow.TopMachine.Data.Repositories;

public class DicoWordsToDicoDataManager<T> : DataManager<T> where T : IDataObject
{
    public DicoWordsToDicoDataManager(DatabaseSettings context) : base(context, "Dictionary", "WordsToDico")
    {
    }
}
