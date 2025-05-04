using Crolow.FastDico.Models.Models.ScrabbleApi.Entities.Partials;
using Kalow.Apps.Common.DataTypes;
using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Models.Models.ScrabbleApi.Entities
{
    public class LetterConfigModel : DataObject
    {
        public string Name { get; set; }
        public List<TileConfig> Letters { get; set; }

        public KalowId DictionaryId { get; set; }

        public LetterConfigModel()
        {
            Id = KalowId.NewObjectId();
        }
    }
}
