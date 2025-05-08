using Crolow.TopMachine.Data.Bridge;

namespace Crolow.TopMachine.Data.Repositories;

public class BoardConfigDataManager<T> : DataManager<T> where T : IDataObject
{
    public BoardConfigDataManager(DatabaseSettings context) : base(context, "TopMachine", "BoardConfigurations")
    {
    }
}
