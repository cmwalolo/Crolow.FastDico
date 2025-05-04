using Crolow.Pix.Core.Services.Storage;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using Kalow.Apps.Common.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
            services.AddScoped<ILetterService, LetterService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IGameConfigService, GameConfigService>();

            LiteDB.BsonMapper.Global.RegisterType<KalowId>
                    (
                        (oid) => new LiteDB.ObjectId(oid.ToByteArray()),
                        (bson) => new KalowId(bson.AsObjectId.ToByteArray())
                    );

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new KalowIdConverter() }
            };
        }
    }
}
