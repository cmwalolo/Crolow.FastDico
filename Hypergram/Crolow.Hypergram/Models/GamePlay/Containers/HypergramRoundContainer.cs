
namespace Kalow.Hypergram.Logic.Models.GamePlay.Containers
{
    public class HypergramRoundContainer
    {
        public HypergramRoundContainer()
        {
            word = new char[15];
            tileorigin = new byte[15];
        }

        public void Init(int rackSource)
        {
            this.rackSource = rackSource;
            for (int x = 0; x < 15; x++)
            {
                word[x] = (char)0;
                tileorigin[x] = 0;
            }

            wordlen = 0;
            points = 0;
            bonus = 0;
        }

        public char[] word;       //[ROUND_INTERNAL_MAX];
        public byte[] tileorigin; //[ROUND_INTERNAL_MAX];

        public int wordlen;
        public int points;
        public int bonus;
        public int rackSource;
    };
}
