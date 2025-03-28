using Kalow.Apps.Common.DataTypes;

namespace Crolow.Pix.Data.Interfaces
{
    public enum EditState
    {
        Unchanged = 0,
        New = 1,
        Update = 2,
        ToDelete = 3
    }
    public interface IDataObject
    {
        EditState EditState { get; set; }
        KalowId Id { get; set; }
    }
}