using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Common.Models.Dictionary.Entities;
public class DictionaryModel : DataObject, IDictionaryModel
{
    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
    public string DictionaryFile { get; set; }

    public List<IDictionaryLookup> DictionaryUrls { get; set; }

    public class DictionaryLookup : IDictionaryLookup
    {
        public string Name { get; set; }
        public string UrlMatch { get; set; }
    }
}
