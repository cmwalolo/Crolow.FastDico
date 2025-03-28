using Crolow.Pix.Data.Interfaces;
using Kalow.Apps.Managers.LiteDb.Data;

namespace Crolow.Pix.Data.Repositories
{
    public class PhotoAnalysisDataManager<T> : DataManager<T> where T : IDataObject
    {
        public PhotoAnalysisDataManager(DatabaseSettings context) : base(context, "Photos", "PhotoAnalysis")
        {
        }
    }



}
