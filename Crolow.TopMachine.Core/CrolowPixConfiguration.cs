using Crolow.Pix.Core.Services.Storage;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Crolow.TopMachine.Core
{
    public static class CrolowPixConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //            services.AddScoped<IAlbumService, AlbumService>();
            //            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IDataFactory, DataFactory>();
            services.AddScoped<IDictionaryService, DictionaryService>();

            LiteDB.BsonMapper.Global.RegisterType<KalowId>
                    (
                        (oid) => new LiteDB.ObjectId(oid.ToByteArray()),
                        (bson) => new KalowId(bson.AsObjectId.ToByteArray())
                    );
        }
    }
}
