using Crolow.TopMachine.Data.Bridge;

namespace Crolow.TopMachine.Data.Repositories;

public class UserDataManager<T> : DataManager<T> where T : IDataObject
{
    public UserDataManager(DatabaseSettings context) : base(context, "TopMachine", "Users")
    {
    }
}
