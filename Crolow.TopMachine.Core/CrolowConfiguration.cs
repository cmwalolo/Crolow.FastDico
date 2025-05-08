using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.Common.Entities;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities.Partials;
using Crolow.Pix.Core.Services.Storage;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Core.Json;
using Crolow.TopMachine.Core.Services.Storage;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using Kalow.Apps.Common.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Crolow.FastDico.Common.Models.Dictionary.Entities.DictionaryModel;
using static Crolow.FastDico.Common.Models.ScrabbleApi.Entities.BoardGridModel;

namespace Crolow.TopMachine.Core
{
    public static class CrolowConfiguration
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
            services.AddScoped<IUserService, UserService>();

            RegisterLiteDbMappers();


            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new KalowIdConverter() },
                ContractResolver = new CustomContractResolver()
            };
        }

        private static void RegisterLiteDbMappers()
        {
            var mapper = new LiteDB.BsonMapper();

            mapper.RegisterType<KalowId>
                    (
                        (oid) => new LiteDB.ObjectId(oid.ToByteArray()),
                        (bson) => new KalowId(bson.AsObjectId.ToByteArray())
                    );

            mapper.RegisterType<IDictionaryModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<DictionaryModel>(bson.AsDocument)
                    );

            mapper.RegisterType<IWordEntryModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<WordEntryModel>(bson.AsDocument)
                    );

            mapper.RegisterType<IWordToDicoModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<WordToDicoModel>(bson.AsDocument)
                    );

            mapper.RegisterType<IDefinitionModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<DefinitionModel>(bson.AsDocument)
                    );

            mapper.RegisterType<IDictionaryLookup>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<DictionaryLookup>(bson.AsDocument)
                    );

            mapper.RegisterType<IBoardGridModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<BoardGridModel>(bson.AsDocument)
                    );

            mapper.RegisterType<IGameConfigModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<GameConfigModel>(bson.AsDocument)
                    );

            mapper.RegisterType<ILetterConfigModel>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<LetterConfigModel>(bson.AsDocument)
                    );

            mapper.RegisterType<IMultiplierData>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<MultiplierData>(bson.AsDocument)
                    );

            mapper.RegisterType<ITileConfig>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<TileConfig>(bson.AsDocument)
                    );

            mapper.RegisterType<IUser>
                    (
                        (obj) => mapper.ToDocument(obj),
                        (bson) => mapper.ToObject<User>(bson.AsDocument)
                    );

            Repository.BsonMapper = mapper;

        }
    }
}
