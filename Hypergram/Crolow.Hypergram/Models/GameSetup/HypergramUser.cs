using Kalow.Apps.Models.Data;

namespace Kalow.Hypergram.Logic.Models.GameSetup
{
    public class HypergramUser : DataObject
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public DateTime LastConnection { get; set; }

    }
}
