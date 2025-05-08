using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Kalow.Apps.Models.Data;
using Newtonsoft.Json;

namespace Crolow.FastDico.Common.Models.Dictionary.Entities
{
    public class WordEntryModel : DataObject, IWordEntryModel
    {
        public string Word { get; set; }
        public string NormalizedWord { get; set; }
        public string Source { get; set; }

        [JsonProperty("ethymology")]
        public string Etymology { get; set; }

        public List<IDefinitionModel> Definitions { get; set; } = new List<IDefinitionModel>();

    }
}
