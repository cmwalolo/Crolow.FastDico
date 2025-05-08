using Crolow.TopMachine.Data.Bridge;
using Kalow.Apps.Common.DataTypes;

namespace Crolow.TopMachine.Data.Bridge.Entities.Definitions
{
    public interface IWordToDicoModel : IDataObject
    {
        KalowId Parent { get; set; }
        string Word { get; set; }
    }
}