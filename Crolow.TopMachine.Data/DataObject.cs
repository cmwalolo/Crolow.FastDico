using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using LiteDB;
using Newtonsoft.Json;

namespace Kalow.Apps.Models.Data
{
    public class DataObject : IDataObject
    {
        public DataObject()
        {
            Id = KalowId.Empty;
            EditState = EditState.Unchanged;
        }
        [JsonProperty("_id")]
        public KalowId Id { get; set; }


        [BsonIgnore]
        public EditState EditState { get; set; }
    }
}