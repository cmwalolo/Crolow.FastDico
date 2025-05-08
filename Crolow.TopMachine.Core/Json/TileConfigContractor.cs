using Crolow.FastDico.Common.Models.ScrabbleApi.Entities.Partials;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Newtonsoft.Json.Serialization;
using static Crolow.FastDico.Common.Models.Dictionary.Entities.DictionaryModel;
using static Crolow.FastDico.Common.Models.ScrabbleApi.Entities.BoardGridModel;

namespace Crolow.TopMachine.Core.Json
{
    public class CustomContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            return objectType switch
            {
                // Map interface types to concrete types
                { } when objectType == typeof(ITileConfig) => base.CreateContract(typeof(TileConfig)),
                { } when objectType == typeof(IMultiplierData) => base.CreateContract(typeof(MultiplierData)),
                { } when objectType == typeof(IDictionaryLookup) => base.CreateContract(typeof(DictionaryLookup)),
                _ => base.CreateContract(objectType) // Default case
            };
        }
    }
}