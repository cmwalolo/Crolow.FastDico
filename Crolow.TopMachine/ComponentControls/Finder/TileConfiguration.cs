using Crolow.FastDico.Models.Models.ScrabbleApi.Entities.Partials;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.Utils;

namespace Crolow.TopMachine.ComponentControls.Finder
{
    public static class TileConfigFactory
    {
        public static void CreateFrenchTileSet()
        {
            var list = new List<TileConfig>
            {
                new TileConfig { Char = "A", Letter = 0,  IsVowel = true,  IsConsonant = false, TotalLetters = 9,  Points = 1 },
                new TileConfig { Char = "B", Letter = 1,  IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 3 },
                new TileConfig { Char = "C", Letter = 2,  IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 3 },
                new TileConfig { Char = "D", Letter = 3,  IsVowel = false, IsConsonant = true,  TotalLetters = 3,  Points = 2 },
                new TileConfig { Char = "E", Letter = 4,  IsVowel = true,  IsConsonant = false, TotalLetters = 15, Points = 1 },
                new TileConfig { Char = "F", Letter = 5,  IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 4 },
                new TileConfig { Char = "G", Letter = 6,  IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 2 },
                new TileConfig { Char = "H", Letter = 7,  IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 4 },
                new TileConfig { Char = "I", Letter = 8,  IsVowel = true,  IsConsonant = false, TotalLetters = 8,  Points = 1 },
                new TileConfig { Char = "J", Letter = 9,  IsVowel = false, IsConsonant = true,  TotalLetters = 1,  Points = 8 },
                new TileConfig { Char = "K", Letter = 10, IsVowel = false, IsConsonant = true,  TotalLetters = 1,  Points = 10 },
                new TileConfig { Char = "L", Letter = 11, IsVowel = false, IsConsonant = true,  TotalLetters = 5,  Points = 1 },
                new TileConfig { Char = "M", Letter = 12, IsVowel = false, IsConsonant = true,  TotalLetters = 3,  Points = 2 },
                new TileConfig { Char = "N", Letter = 13, IsVowel = false, IsConsonant = true,  TotalLetters = 6,  Points = 1 },
                new TileConfig { Char = "O", Letter = 14, IsVowel = true,  IsConsonant = false, TotalLetters = 6,  Points = 1 },
                new TileConfig { Char = "P", Letter = 15, IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 3 },
                new TileConfig { Char = "Q", Letter = 16, IsVowel = false, IsConsonant = true,  TotalLetters = 1,  Points = 8 },
                new TileConfig { Char = "R", Letter = 17, IsVowel = false, IsConsonant = true,  TotalLetters = 6,  Points = 1 },
                new TileConfig { Char = "S", Letter = 18, IsVowel = false, IsConsonant = true,  TotalLetters = 6,  Points = 1 },
                new TileConfig { Char = "T", Letter = 19, IsVowel = false, IsConsonant = true,  TotalLetters = 6,  Points = 1 },
                new TileConfig { Char = "U", Letter = 20, IsVowel = true,  IsConsonant = false, TotalLetters = 6,  Points = 1 },
                new TileConfig { Char = "V", Letter = 21, IsVowel = false, IsConsonant = true,  TotalLetters = 2,  Points = 4 },
                new TileConfig { Char = "W", Letter = 22, IsVowel = false, IsConsonant = true,  TotalLetters = 1,  Points = 10 },
                new TileConfig { Char = "X", Letter = 23, IsVowel = false, IsConsonant = true,  TotalLetters = 1,  Points = 10 },
                new TileConfig { Char = "Y", Letter = 24, IsVowel = true,  IsConsonant = true,  TotalLetters = 1,  Points = 10 },
                new TileConfig { Char = "Z", Letter = 25, IsVowel = false, IsConsonant = true,  TotalLetters = 1,  Points = 10 },
                new TileConfig { Char = "?", Letter = 252, IsVowel = true, IsConsonant = true, TotalLetters = 2, Points = 0, IsJoker = true }
            };

            var config = new BagConfiguration();
            config.Name = "FR";
            foreach (var letter in list)
            {
                config.LettersByByte.Add(letter.Letter, letter);
                config.LettersByChar.Add(letter.Char[0], letter);
            }
            TilesUtils.configuration = config;

        }
    }
}


