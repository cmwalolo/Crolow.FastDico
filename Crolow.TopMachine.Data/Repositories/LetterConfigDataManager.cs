using Crolow.TopMachine.Data.Bridge;

namespace Crolow.TopMachine.Data.Repositories;

public class LetterConfigDataManager<T> : DataManager<T> where T : IDataObject
{
    public LetterConfigDataManager(DatabaseSettings context) : base(context, "TopMachine", "LetterConfigurations")
    {
    }
}
