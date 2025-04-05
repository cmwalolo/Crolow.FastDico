using Crolow.FastDico.Models.Models.ScrabbleApi.Entities.Partials;
using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Models.Models.ScrabbleApi.Entities
{
    public class LetterData : DataObject
    {
        public string Name { get; set; }
        public List<TileConfig> Letters { get; set; }
    }
}
