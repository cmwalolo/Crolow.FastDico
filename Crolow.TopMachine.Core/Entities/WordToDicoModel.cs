using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Kalow.Apps.Common.DataTypes;
using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Common.Models.Dictionary.Entities
{
    public class WordToDicoModel : DataObject, IWordToDicoModel
    {
        public string Word { get; set; }
        public KalowId Parent { get; set; } = KalowId.Empty;
    }
}
