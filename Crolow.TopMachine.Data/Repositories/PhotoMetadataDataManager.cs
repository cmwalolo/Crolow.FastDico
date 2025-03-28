using Crolow.Pix.Data.Interfaces;
using Kalow.Apps.Managers.LiteDb.Data;

namespace Crolow.Pix.Data.Repositories
{
    public class PhotoMetadataDataManager<T> : DataManager<T> where T : IDataObject
    {
        public PhotoMetadataDataManager(DatabaseSettings context) : base(context, "Photos", "PhotoMetadata")
        {

        }
    }
}
