using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi.Config
{
    public class BagConfiguration
    {
        public string Name { get; set; }
        public Dictionary<byte, TileConfig> LettersByByte { get; set; } = new Dictionary<byte, TileConfig>();
        public Dictionary<char, TileConfig> LettersByChar { get; set; } = new Dictionary<char, TileConfig>();
    }

}
