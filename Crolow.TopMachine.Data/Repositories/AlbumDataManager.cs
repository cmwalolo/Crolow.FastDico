using Crolow.Pix.Data.Interfaces;
using Kalow.Apps.Managers.LiteDb.Data;

namespace Crolow.Pix.Data.Repositories
{
    public class AlbumDataManager<T> : DataManager<T> where T : IDataObject
    {
        public AlbumDataManager(DatabaseSettings context) : base(context, "Photos", "Albums")
        {
            // repository.EnsureCollection("Albums");
        }
    }
}
