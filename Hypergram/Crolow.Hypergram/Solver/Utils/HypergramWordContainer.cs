using Kalow.Hypergram.Core.Dawg;

namespace Kalow.Hypergram.Core.Solver.Utils
{
    public class HypergramWordContainer
    {
        public char[] WordTiles { get; set; }
        public int[] SourceOfTiles { get; set; }
        public bool[] JokerTiles { get; set; }

        public string Word { get; set; } = string.Empty;
        public int WordLength { get; set; } = 0;
        public int CurrentMultiplier { get; set; } = 1;
        public int NextMultiplyTurn { get; set; }
        public int WordScore { get; set; }
        public int LastRound { get; set; }

        public HypergramWordContainer()
        {
            ClearContainer();
        }

        public void ClearContainer()
        {
            WordTiles = new char[DicoConstants.LETTERS];
            SourceOfTiles = new int[DicoConstants.LETTERS];
            JokerTiles = new bool[DicoConstants.LETTERS];
            WordLength = 0;
            CurrentMultiplier = 1;
        }


        public void AddToWord(char letter)
        {
            Word += letter;
            JokerTiles[WordLength] = char.IsLower(letter);
            WordTiles[WordLength] = letter;
            WordLength++;
        }


        public void SetNewWord(string newWord)
        {
            ClearContainer();
            Word = newWord;
            WordLength = newWord.Length;
            for (int x = 0; x < newWord.Length; x++)
            {
                var c = newWord[x];
                JokerTiles[x] = char.IsLower(c);
                WordTiles[x] = c;
            }
        }

        public void SetNewWord(string newWord, int points)
        {
            SetNewWord(newWord);
            WordScore = points;
        }

        public void SetNewWord(char[] newWord)
        {
            ClearContainer();
            Word = string.Empty;
            for (int x = 0; x < newWord.Length; x++)
            {
                var c = newWord[x];
                JokerTiles[x] = char.IsLower(c);
                WordTiles[x] = c;
            }
            WordLength = newWord.Length;
        }

        public string GetWord()
        {
            return Word;
        }

        public int GetCurrentMultiplier()
        {
            return CurrentMultiplier;
        }

        public bool IsJoker(int x)
        {
            return JokerTiles[x];
        }

        public char GetLetter(int x)
        {
            return WordTiles[x];
        }

        public int GetScore()
        {
            return WordScore * CurrentMultiplier;
        }

    }
}
