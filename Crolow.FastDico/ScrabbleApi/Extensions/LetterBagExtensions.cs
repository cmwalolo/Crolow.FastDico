using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Utils;
using Crolow.FastDico.ScrabbleApi.Extensions;
using Tile = Crolow.FastDico.Common.Models.ScrabbleApi.Tile;

namespace Crolow.FastDico.ScrabbleApi.Extensions;

public static class LetterBagExtensions
{
    static Random RandomGen = Random.Shared;

    // Draw a specified number of letters from the bag
    public static List<Tile> DrawLetters(this LetterBag b, PlayerRack rack, int totalLetters = 0)
    {
        int count = totalLetters == 0 ? b.GameConfig.SelectedConfig.InRackLetters : totalLetters;
        var drawnLetters = rack.GetTiles();

        bool ok = true;

        if (!b.IsValid(b.Letters, drawnLetters))
        {
            return null;
        }

        do
        {
            if (!ok)
            {
                b.ReturnLetters(rack, drawnLetters);
                drawnLetters = rack.GetTiles();
            }

            if (b.GameConfig.SelectedConfig.JokerMode)
            {
                if (!drawnLetters.Any(p => p.IsJoker == true))
                {
                    var ndx = b.Letters.FindIndex(p => p.IsJoker);
                    if (ndx > 0)
                    {
                        drawnLetters.Add(b.Letters[ndx]);
                        b.Letters.RemoveAt(ndx);
                    }
                }
            }

            for (int i = drawnLetters.Count; i < count; i++)
            {
                if (!b.IsEmpty)
                {
                    int index = RandomGen.Next(b.RemainingLetters);
                    var l = b.Letters[index];
                    if (!b.GameConfig.SelectedConfig.JokerMode || b.GameConfig.SelectedConfig.JokerMode && !l.IsJoker)
                    {
                        drawnLetters.Add(b.Letters[index]);
                        b.Letters.RemoveAt(index);
                    }
                    continue;
                }
                break;
            }
            ok = b.IsValid(drawnLetters, null);
        } while (!ok);
        return drawnLetters;

    }

    public static void ReturnLetters(this LetterBag b, PlayerRack rack)
    {
        b.Letters.AddRange(rack.GetTiles());
        rack.Clear();
    }

    public static void ReturnLetters(this LetterBag b, PlayerRack rack, List<Tile> drawnLetters)
    {
        b.Letters.AddRange(drawnLetters);
        rack.Clear();
    }

    public static void RemoveTile(this LetterBag b, Tile tile)
    {
        var i = b.Letters.FindIndex(t => !tile.IsJoker && !t.IsJoker && t.Letter == tile.Letter || t.IsJoker && tile.IsJoker);
        if (i == -1)
        {
            Console.WriteLine("missing tile");
            return;
        }
        b.Letters.RemoveAt(i);
    }

    // Get the number of letters remaining in the bag

    public static bool IsValid(this LetterBag b, List<Tile> letters, List<Tile> rack)
    {
        var tileConfig = b.GameConfig.BagConfig;

        int vow = letters.Sum(p => tileConfig.LettersByByte[p.Letter].IsVowel ? 1 : 0) + (rack?.Sum(p => tileConfig.LettersByByte[p.Letter].IsVowel ? 1 : 0) ?? 0);
        int con = letters.Sum(p => tileConfig.LettersByByte[p.Letter].IsConsonant ? 1 : 0) + (rack?.Sum(p => tileConfig.LettersByByte[p.Letter].IsConsonant ? 1 : 0) ?? 0);
        int jok = letters.Sum(p => tileConfig.LettersByByte[p.Letter].IsJoker ? 1 : 0) + (rack?.Sum(p => tileConfig.LettersByByte[p.Letter].IsJoker ? 1 : 0) ?? 0);

        if (b.CurrentGame.Round >= b.CurrentGame.Configuration.SelectedConfig.CheckDistributionRound)
        {
            return vow > 0 && con > 0;
        }

        if (b.CurrentGame.GameConfig.JokerMode && b.Letters.Count > 7)
        {
            return vow - jok > 1 && con - jok > 1;
        }
        else
        {
            return vow > 1 && con > 1;
        }
    }

    public static void ForceDrawLetters(this LetterBag b, List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            var i = b.Letters.FindIndex(t => !tile.IsJoker && !t.IsJoker && t.Letter == tile.Letter || t.IsJoker && tile.IsJoker);
            if (i != -1)
            {
                b.Letters.RemoveAt(i);
            }
        }
    }

    public static Tile ReplaceJoker(this LetterBag b, Tile tile)
    {
        if (tile.IsJoker)
        {
            var ndx = b.Letters.FindIndex(p => p.Letter == tile.Letter);
            if (ndx != -1)
            {
                var newTile = b.Letters[ndx];
                b.Letters.RemoveAt(ndx);
                // Do not change this !!! 
                // Any where you find new Tile(tile) 
                // to make sure that referenced objects are not modified.
                b.Letters.Add(new Tile(tile, tile.Parent));
                return newTile;
            }
        }

        return tile;
    }

    public static void Recreate(this LetterBag b, PlayerRack rack, PlayerRack originalRack)
    {
        rack.Clear();
        foreach (var t in originalRack.Tiles)
        {
            rack.Tiles.Add(t);
            b.RemoveTile(t);
        }
    }

    public static void DebugBag(this LetterBag b, PlayerRack rack)
    {
#if DEBUG
        Console.WriteLine($"Player rack : {rack.GetString()}");
        var g = b.Letters.GroupBy(p => p.IsJoker ? TilesUtils.JokerByte : p.Letter).OrderBy(p => p.Key);
        Console.Write("BAG : ");
        foreach (var l in g)
        {
            char c = l.Key == TilesUtils.JokerByte ? '?' : (char)(l.Key + 65);
            Console.Write($"{c}: {l.Count()} -");
        }
        Console.WriteLine();
        Console.WriteLine($"Letter count : {b.Letters.Count}");
        Console.WriteLine("------------------------");

#endif
    }
}
