using Crolow.TopMachine.Data.Bridge;

namespace Crolow.TopMachine.Data.Repositories;
public class GameConfigDataManager<T> : DataManager<T> where T : IDataObject
{
    public GameConfigDataManager(DatabaseSettings context) : base(context, "TopMachine", "GameConfigurations")
    {
        //base.Repository.EnsureCollection("GameConfigs");
    }
}
