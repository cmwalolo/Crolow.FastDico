using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Components.Rounds;
using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi.Extensions;

public static class PlayedRoundsExtensions
{
    public static void SetRound(this PlayedRounds p, PlayableSolution round)
    {
        int wm = 1;
        int pivotTotal = 0;
        foreach (var t in round.Tiles)
        {
            if (t.Parent.Status == 0)
            {
                wm *= t.Parent.WordMultiplier;
                if (t.PivotPoints > 0)
                {
                    pivotTotal += t.Points * t.Parent.LetterMultiplier * t.Parent.WordMultiplier + t.PivotPoints * t.Parent.WordMultiplier;
                }
            }
        }

        round.Points = round.Tiles.Where(p => p.Parent.Status == 0).Sum(p => p.Points * p.Parent.LetterMultiplier) * wm;
        round.Points += round.Tiles.Where(p => p.Parent.Status == 1).Sum(p => p.Points) * wm;

        var tilesFromRack = round.Tiles.Count(p => p.Parent.Status == 0);
        if (tilesFromRack > 0 && tilesFromRack < p.Config.Bonus.Count())
        {
            round.Bonus = p.Config.Bonus[tilesFromRack - 1];

        }

        round.Points += round.Bonus + pivotTotal;

        if (p.PickAll)
        {
            p.AllRounds.Add(round);
        }

        if (round.Points > p.MaxPoints)
        {
            p.SubTops = p.Tops;
            p.Tops = new List<PlayableSolution>();
            p.Tops.Add(round);
            p.MaxSubTopPoints = p.SubTops.Any() ? p.SubTops[0].Points : 0;
            p.MaxPoints = round.Points;
        }
        else
        {
            if (round.Points > p.MaxSubTopPoints)
            {
                p.MaxSubTopPoints = round.Points;
                p.SubTops.Clear();
                p.SubTops.Add(round);
            }
            return;
        }

        round.Tiles = round.Tiles.Select(p => new Tile(p, p.Parent)).ToList();
    }
}
