
namespace Kalow.Hypergram.Logic.Models.GamePlay.Containers
{
    public class HypergramRackContainer
    {
        public int[] _ttiles;
        public int[] _tiles;

        public int[] ttiles
        {
            get { return _ttiles; }
        }

        public int[] tiles
        {
            get { return _tiles; }
        }

        public int ntiles;

        public HypergramRackContainer()
        {
            _ttiles = new int[16];
            _tiles = new int[16];
        }
    };
}
