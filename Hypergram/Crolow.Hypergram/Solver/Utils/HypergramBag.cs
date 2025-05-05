using Kalow.Hypergram.Core.Dawg;
using Kalow.Hypergram.Logic.Models.GamePlay.Containers;
using Kalow.Hypergram.Logic.Models.GameSetup;

namespace Kalow.Hypergram.Core.Solver.Utils
{
    public class HypergramBag
    {
        public HypergramBagContainer tbag;


        protected Random rand;
        protected HypergramBoardConfig cm;
        protected HypergramConfig config;

        public void copy(HypergramBag src)
        {
            for (int x = 0; x < DicoConstants.LETTERS; x++)
            {
                tbag.tiles[x] = src.tbag.tiles[x];
            }

            tbag.ntiles = src.tbag.ntiles;
            tbag.totaltiles = src.tbag.totaltiles;

            tbag.maptiles = new int[tbag.totaltiles];
            tbag.maptilesF = new int[tbag.totaltiles];


            for (int x = 0; x < tbag.totaltiles; x++)
            {
                tbag.maptiles[x] = src.tbag.maptiles[x];
                tbag.maptilesF[x] = src.tbag.maptilesF[x];
            }
        }

        public HypergramBag(HypergramBoardConfig cm, HypergramConfig config)
        {
            rand = new Random();
            tbag = new HypergramBagContainer();
            this.cm = cm;
            this.config = config;
            Init();
        }


        public void Init()
        {
            int i;
            tbag.ntiles = 0;

            int c = 0;

            var tileQ = cm.GetTileCode('Q');
            var tileW = cm.GetTileCode('W');

            for (i = 0; i < cm.config.NbLetters; i++)
            {
                if (i == tileQ || i == tileW)
                {
                    if (config.NumberOfBags > 2 && i == tileQ)
                    {
                        tbag.ntiles += 2;
                    }
                    else
                        tbag.ntiles += 1;

                }
                else
                {
                    tbag.ntiles += cm.config.Tiles_numbers[i] * config.NumberOfBags;
                }
            }

            tbag.totaltiles = tbag.ntiles;

            tbag.maptiles = new int[tbag.totaltiles];
            tbag.maptilesF = new int[tbag.totaltiles];

            for (i = 0; i < cm.config.NbLetters; i++)
            {
                int amountLetters = cm.config.Tiles_numbers[i] * config.NumberOfBags;

                if (i == tileQ || i == tileW)
                {
                    if (config.NumberOfBags > 2 && i == tileQ)
                    {
                        amountLetters = 2;
                    }
                    else
                    {
                        amountLetters = 1;
                    }
                }

                tbag.tiles[i] = amountLetters;
                for (int x = 0; x < amountLetters; x++)
                {
                    tbag.maptiles[c] = i;
                    tbag.maptilesF[c] = 1;
                    c++;
                }
            }
            Shuffle(tbag.maptiles);
        }

        public void Shuffle(int[] array)
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                // Swap elements array[i] and array[j]
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        public void Clear()
        {
            for (int x = 0; x < DicoConstants.LETTERS; x++)
            {
                tbag.tiles[x] = 0;
            }

            for (int i = 0; i < tbag.totaltiles; i++)
            {
                tbag.maptiles[i] = 0;
                tbag.maptilesF[i] = 0;
            }
        }
        public int SelectRandom()
        {
            int i, n;
            do
            {
                n = rand.Next(tbag.ntiles + 1); //  TILES_TOTAL;
            } while (n >= tbag.ntiles);

            int x = 0;
            int found = 0;
            while (true)
            {
                if (tbag.maptilesF[x] == 1)
                {
                    if (n == found)
                    {
                        return tbag.maptiles[x];
                    }
                    found++;
                }
                x++;
            }
        }

        public int IsIn(int c)
        {
            return tbag.tiles[c];
        }


        public int Ntiles()
        {
            return tbag.ntiles;
        }

        private int GetTileAtPosition(int x)
        {
            int pos = 0;
            int found = 0;
            while (true)
            {
                if (tbag.maptilesF[pos] == 1)
                {
                    if (x == found) return tbag.maptiles[pos];
                    found++;
                }
                pos++;
            }
            return -1;
        }

        public int TakeTile(int t)
        {
            if (IsIn(t) > 0)
            {
                tbag.tiles[t]--;
                tbag.ntiles--;

                for (int x = 0; x < tbag.totaltiles; x++)
                {
                    if (tbag.maptiles[x] == t && tbag.maptilesF[x] == 1)
                    {
                        tbag.maptilesF[x] = 0;
                        break;
                    }
                }
            }
            else
            {
                return 1;
            }
            return 0;
        }

        public void GetTotalTiles(out int vowel, out int consonant, out int joker)
        {
            int i;
            int v = 0;
            int c = 0;
            int j = 0;
            // -1 : eviter de comptabilise le joker pour voyelle et consonne
            for (i = 0; i < cm.config.TotalTiles - 1; i++)
            {
                if (tbag.maptilesF[i] == 1)
                {
                    if (tbag.maptiles[i] == DicoConstants.JOKER_TILE)
                        j++;
                    else
                    {
                        v += cm.config.Tiles_vowels[tbag.maptiles[i]];
                        c += cm.config.Tiles_consonants[tbag.maptiles[i]];
                    }
                }
            }
            joker = j;
            vowel = v;
            consonant = c;

        }

        private int check()
        {
            int c = 0;
            for (int x = 0; x < tbag.totaltiles; x++)
            {
                if (tbag.maptilesF[x] == 1) c++;
            }
            return c;
        }

        public int ReplaceTitle(int t)
        {
            tbag.tiles[t]++;
            tbag.ntiles++;

            for (int x = 0; x < tbag.totaltiles; x++)
            {
                if (tbag.maptiles[x] == t && tbag.maptilesF[x] == 0)
                {
                    tbag.maptilesF[x] = 1;
                    break;
                }
            }
            return 1;
        }

    }
}
