using Kalow.Hypergram.Core.Dawg;
using Kalow.Hypergram.Logic.Models.GamePlay.Containers;

namespace Kalow.Hypergram.Core.Solver.Utils
{

    public class HypergramRack
    {
        HypergramBoardConfig cm;
        HypergramRackContainer track;

        public HypergramRack(HypergramBoardConfig cm)
        {
            this.cm = cm;
            Init();
            track = new HypergramRackContainer();
        }

        public void Copy(int[] words)
        {
            track._ttiles = new int[DicoConstants.LETTERS];
            track._tiles = new int[DicoConstants.LETTERS];
            for (int x = 0; x < words.Length; x++)
            {
                Add(words[x]);
            }
            AlignTiles();
        }


        public void Copy(HypergramRack source)
        {
            track._ttiles = new int[DicoConstants.LETTERS];
            track._tiles = new int[DicoConstants.LETTERS];

            for (int x = 0; x < DicoConstants.LETTERS; x++)
            {
                track._ttiles[x] = source.track._ttiles[x];
                track._tiles[x] = source.track._tiles[x];
            }

            track.ntiles = source.track.ntiles;
        }


        public void SetRack(string word)
        {
            foreach (var c in word)
            {
                Add(cm.GetTileCode(c));
            }
        }

        public void Init()
        {
            track = new HypergramRackContainer();
        }

        public int Empty()
        {
            track = new HypergramRackContainer();
            return track.ntiles;
        }

        public int Ntiles()
        {
            return track.ntiles;
        }


        public int IsIn(int tile)
        {
            if (tile >= track._tiles.Length)
            {
                return 0;
            }
            else
            {
                return track._tiles[tile];
            }
        }

        public void Remove(int tile)
        {
            track._tiles[tile]--;
            track.ntiles--;
        }


        public void AlignTiles()
        {
            int c = 0;

            track._ttiles = new int[DicoConstants.LETTERS];
            for (int x = 0; x < cm.config.NbLetters; x++)
            {
                for (int y = 0; y < track._tiles[x]; y++)
                {
                    track._ttiles[c] = x;
                    c++;
                }
            }
        }

        public void UnalignTiles()
        {
            track._tiles = new int[DicoConstants.LETTERS];

            for (int x = 0; x < track.ntiles; x++)
            {
                track._tiles[track._ttiles[x]]++;
            }
        }

        public void Add(int tile)
        {
            track._tiles[tile]++;
            track.ntiles++;
        }

        public int GetRackTile(int n)
        {
            return track._ttiles[n];
        }

        public char GetRackAscii(int n)
        {
            return cm.config.Tiles_ascii[track._ttiles[n]];
        }

        public string GetRackString()
        {
            string s = "";

            for (int i = 0; i < Ntiles(); i++)
            {
                s += GetRackAscii(i);
            }
            return s;
        }

        public int CheckRack(int n, int nbl)
        {
            int v, c, j;
            GetTotalTiles(out v, out c, out j);
            return v >= n && c >= n ? 1 : 0;
        }

        public void GetTotalTiles(out int vowel, out int consonant, out int joker)
        {
            int i;
            int v = 0;
            int c = 0;

            for (i = 1; i < cm.config.NbLetters; i++)
            {
                if (track._tiles[i] > 0)
                {
                    v += cm.config.Tiles_vowels[i] * track._tiles[i];
                    c += cm.config.Tiles_consonants[i] * track._tiles[i];
                }
            }

            joker = track._tiles[0];
            vowel = v;
            consonant = c;
        }

    }
}
