using Kalow.Apps.Models.Data;
using Newtonsoft.Json;

namespace Crolow.FastDico.Models.Models.Dictionary.Entities
{
    public class DefinitionModel
    {
        public string Word { get; set; }
        public string CatGram { get; set; } = string.Empty;
        public List<string> Definitions { get; set; } = new List<string>();
        public List<string> Domains { get; set; } = new List<string>();
        public List<string> Usages { get; set; } = new List<string>();
    }

    public class WordEntryModel : DataObject
    {
        public string Word { get; set; }
        public string NormalizedWord { get; set; }
        public string Source { get; set; }

        [JsonProperty("ethymology")]
        public string Etymology { get; set; }

        public List<DefinitionModel> Definitions { get; set; } = new List<DefinitionModel>();

    }
}
