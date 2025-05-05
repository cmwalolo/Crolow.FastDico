using Kalow.Hypergram.Core.Dawg;
using Kalow.Hypergram.Logic.Models.GamePlay.Containers;

namespace Kalow.Hypergram.Core.Solver.Utils
{
    public class HypergramBoardConfig
    {

        public HypergramBoardConfigContainer config = new HypergramBoardConfigContainer();
        public void SetTiles(int size, int[] vowels, int[] consonants, int[] numbers,
            int[] points, char[] ascii)
        {
            config.NbLetters = size;
            config.JokerTitle = size - 1;
            config.TotalTiles = 0;
            config.Tiles_vowels = new int[size];
            config.Tiles_consonants = new int[size];
            config.Tiles_numbers = new int[size];
            config.Tiles_points = new int[size];
            config.Tiles_ascii = new char[size];
            for (int x = 0; x < size; x++)
            {
                config.Tiles_vowels[x] = vowels[x];
                config.Tiles_consonants[x] = consonants[x];
                config.Tiles_numbers[x] = numbers[x];
                config.Tiles_points[x] = points[x];
                config.Tiles_ascii[x] = ascii[x];
                config.TotalTiles += config.Tiles_numbers[x];
            }
        }

        public char GetTileAsciiChar(int x)
        {
            return config.Tiles_ascii[x];
        }
        public int GetTileCode(char x)
        {
            if (char.IsUpper(x))
            {
                for (int y = 0; y < config.NbLetters; y++)
                {
                    if (config.Tiles_ascii[y] == x)
                    {
                        return y;
                    }
                }
            }
            return DicoConstants.JOKER_TILE;
        }

        public int GetTilePoints(int x)
        {
            return config.Tiles_points[x];
        }

        public int GetTilePointsFromChar(char x)
        {
            return char.IsLower(x) ? 0 : GetTilePoints(GetTileCode(x));
        }

        public int GetTilePointsFromString(string wordPlayed)
        {
            int points = 0;
            foreach (var c in wordPlayed)
            {
                points += GetTilePointsFromChar(c);
            }

            return points;
        }
    }
}
