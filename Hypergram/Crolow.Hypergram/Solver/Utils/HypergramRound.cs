using Kalow.Hypergram.Core.Dawg;
using Kalow.Hypergram.Logic.Models.GamePlay.Containers;

namespace Kalow.Hypergram.Core.Solver.Utils
{
    public class HypergramRound
    {
        protected System.Collections.ArrayList ErrorList;
        protected HypergramBoardConfig cm;
        protected int RemoveAfterRound;

        public HypergramRoundContainer Round { get; set; }

        public HypergramRound(HypergramBoardConfig cm)
        {
            this.cm = cm;
            Create(0);
        }

        public HypergramRound(HypergramBoardConfig cm, int rackSource)
        {
            this.cm = cm;
            Create(rackSource);
        }

        public HypergramRoundContainer Create(int rackSource)
        {
            Round = new HypergramRoundContainer(); // (struct tround *)malloc(sizeof(struct tround));
            Init(rackSource);
            return Round;
        }

        public void Init(int rackSource)
        {
            ErrorList = new System.Collections.ArrayList();
            Round.Init(rackSource);
        }

        public void Copy(HypergramRoundContainer source)
        {
            Round.bonus = source.bonus;
            Round.points = source.points;

            for (int x = 0; x < DicoConstants.ROUND_INTERNAL_MAX; x++)
            {
                Round.word[x] = source.word[x];
                Round.tileorigin[x] = source.tileorigin[x];
            }
            Round.wordlen = source.wordlen;
            Round.rackSource = source.rackSource;
        }

        public void SetWord(char[] c)
        {
            for (int x = 0; x < DicoConstants.ROUND_INTERNAL_MAX; x++)
            {
                Round.word[x] = c[x];
            }
        }


        public void SetWord(string s)
        {
            char[] path = s.ToCharArray();

            for (int x = 0; x < s.Length; x++)
            {
                path[x] = (char)cm.GetTileCode(path[x]);
            }

            Array.Copy(path, Round.word, s.Length);
            Round.wordlen = s.Length;
        }

        public string GetWordWithJoker()
        {
            char[] word = new char[DicoConstants.ROUND_INTERNAL_MAX];

            int l = Round.wordlen;
            for (int x = 0; x < l; x++)
            {
                if (Round.word[x] != 0)
                {
                    if ((Round.tileorigin[x] & 4) == 4)
                        word[x] = (char)(Round.word[x] + (char)96);
                    else
                        word[x] = (char)(Round.word[x] + (char)64);
                }
                else
                    word[x] = '?';
            }

            return new string(word, 0, l);
        }

        public string GetWord()
        {
            char[] word = new char[DicoConstants.ROUND_INTERNAL_MAX];

            for (int x = 0; x < Round.wordlen; x++)
            {
                word[x] = (char)(Round.word[x] + (char)64);
            }

            return new string(word, 0, Round.wordlen);
        }

        public void SetPoints(int p)
        {
            Round.points = p;
        }

        public void GetPoints(int p)
        {
            Round.points = p;
        }

        public void SetPoints()
        {
            var points = 0;
            for (int x = 0; x < WordLen(); x++)
            {
                if (Joker(x) != DicoConstants.JOKER)
                {
                    var tile = GetTile(x);
                    points += cm.GetTilePoints(tile);
                }
            }
            SetPoints(points * WordLen());
        }


        public void SetBonus(int b, int[] intBonus)
        {
            Round.bonus = intBonus[b];
        }

        public char GetTile(int n)
        {
            return Round.word[n];
        }

        public byte Joker(int c)
        {
            return (byte)(Round.tileorigin[c] & DicoConstants.JOKER);
        }


        public int PlayedFromRack(int c)
        {
            return Round.tileorigin[c] & DicoConstants.FROMRACK;
        }

        public int CountFromRack()
        {
            int c = 0;
            for (int x = 0; x < Round.wordlen; x++)
            {
                c += (Round.tileorigin[x] & DicoConstants.FROMRACK) > 1 ? 1 : 0;
            }

            return c;
        }

        public int CountFromBoard()
        {
            int c = 0;
            for (int x = 0; x < Round.wordlen; x++)
            {
                c += (Round.tileorigin[x] & DicoConstants.FROMBOARD) == 1 ? 1 : 0;
            }

            return c;
        }


        public int WordLen()
        {
            return Round.wordlen;
        }


        public int GetPoints()
        {
            return Round.points;
        }

        public int GetBonus()
        {
            return Round.bonus;
        }


        public void AddRightFromBoard(char c)
        {
            Round.word[Round.wordlen] = c;
            Round.tileorigin[Round.wordlen++] = DicoConstants.FROMBOARD;
        }

        public void RemoveRightToBoard()
        {
            Round.wordlen--;
            Round.word[Round.wordlen] = (char)0;
            Round.tileorigin[Round.wordlen] = 0;
        }

        public void AddRightFromRack(char c, int j)
        {
            Round.word[Round.wordlen] = c;
            Round.tileorigin[Round.wordlen] = DicoConstants.FROMRACK;
            if (j == 1)
            {
                Round.tileorigin[Round.wordlen] |= DicoConstants.JOKER;
            }
            Round.wordlen++;
        }

        public void RemoveRightToRack()
        {
            Round.wordlen--;
            Round.word[Round.wordlen] = (char)0;
            Round.tileorigin[Round.wordlen] = 0;
        }

        public void SetTileOrigin(int letter, int x)
        {
            Round.tileorigin[letter] = (byte)x;
        }

        public int GetTileOrigin(int letter)
        {
            return Round.tileorigin[letter];
        }


        public bool Compare(HypergramRound r)
        {
            if (r.Round.wordlen == Round.wordlen)
            {
                string s = GetWord();
                string t = r.GetWord();
                if (s.CompareTo(t) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetRackPlayed()
        {
            return Round.rackSource;
        }

        public bool GetAndRemoveLetter(char letter)
        {
            bool result = false;
            int c = 0;

            char[] word = new char[DicoConstants.ROUND_INTERNAL_MAX];       //[ROUND_INTERNAL_MAX];
            byte[] tileorigin = new byte[DicoConstants.ROUND_INTERNAL_MAX]; //[ROUND_INTERNAL_MAX];


            for (int x = 0; x < Round.wordlen; x++)
            {
                if (!result && Round.word[x] == letter)
                {
                    result = true;
                }
                else
                {
                    word[c] = Round.word[x];
                    tileorigin[c] = Round.tileorigin[x];
                    c++;
                }
            }

            if (result)
            {
                Round.word = word;
                Round.tileorigin = tileorigin;
                Round.wordlen--;
            }
            return result;
        }

        public string GetFromRack()
        {
            string result = string.Empty;
            for (int x = 0; x < Round.wordlen; x++)
            {
                if (GetTileOrigin(x) > 1)
                {
                    result += cm.GetTileAsciiChar(Round.word[x]);
                }
            }
            return result;
        }
    }
}
