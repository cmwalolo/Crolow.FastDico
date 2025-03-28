using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.ScrabbleApi.Utils;
public class BoardGrid : DataObject
{
    public class MultiplierData
    {
        public int Multiplier { get; set; }
        public List<int[]> Positions { get; set; }
    }

    public string Name { get; set; }
    public int SizeH { get; set; }
    public int SizeV { get; set; }
    public List<MultiplierData> Configuration { get; set; }
}
