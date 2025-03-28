using Crolow.Pix.Data.Interfaces;
using Kalow.Apps.Managers.LiteDb.Data;

namespace Crolow.Pix.Data.Repositories
{
    public class PhotoDataManager<T> : DataManager<T> where T : IDataObject
    {
        public PhotoDataManager(DatabaseSettings context) : base(context, "Photos", "Photos")
        {
        }
    }



}
