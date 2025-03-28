using Crolow.Pix.Data.Interfaces;
using Kalow.Apps.Managers.LiteDb.Data;

namespace Crolow.Pix.Data.Repositories
{
    public class ThumbnailsDataManager<T> : DataManager<T> where T : IDataObject
    {
        public ThumbnailsDataManager(DatabaseSettings context) : base(context, "Photos", "Thumbnails")
        {
        }
    }
}
