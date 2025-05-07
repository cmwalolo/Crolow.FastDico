using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Common.Models.Dictionary.Entities;
public class DictionaryModel : DataObject
{
    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
    public string DictionaryFile { get; set; }

    public List<DictionaryLookup> DictionaryUrls { get; set; }

    public class DictionaryLookup
    {
        public string Name { get; set; }
        public string UrlMatch { get; set; }
    }
}
