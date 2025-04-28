using Crolow.FastDico.Utils;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi.Components.Rounds;

public class PlayedRounds
{
    GameConfigModel Config { get; set; }
    public int MaxPoints { get; set; }
    public int MaxSubTopPoints { get; set; }
    public bool PickAll { get; set; }

    public List<PlayedRound> Tops { get; set; }
    public List<PlayedRound> SubTops { get; set; }
    public List<PlayedRound> AllRounds { get; set; }
    public PlayedRound CurrentRound { get; set; }

    public PlayerRack PlayerRack { get; set; }

    public PlayedRounds(GameConfigModel config, List<Tile> rack)
    {
        Config = config;
        Tops = new List<PlayedRound>();
        SubTops = new List<PlayedRound>();
        AllRounds = new List<PlayedRound>();
        CurrentRound = new PlayedRound();
        PlayerRack = new PlayerRack(rack);
    }
    public void SetRound(PlayedRound round)
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
        if (tilesFromRack > 0 && tilesFromRack < Config.Bonus.Count())
        {
            round.Bonus = Config.Bonus[tilesFromRack - 1];

        }

        round.Points += round.Bonus + pivotTotal;

        if (PickAll)
        {
            AllRounds.Add(round);
        }

        if (round.Points > MaxPoints)
        {
            SubTops = Tops;
            Tops = new List<PlayedRound>();
            Tops.Add(round);
            MaxSubTopPoints = SubTops.Any() ? SubTops[0].Points : 0;
            MaxPoints = round.Points;
        }
        else
        {
            if (round.Points > MaxSubTopPoints)
            {
                MaxSubTopPoints = round.Points;
                SubTops.Clear();
                SubTops.Add(round);
            }
            return;
        }

        round.Tiles = round.Tiles.Select(p => new Tile(p, p.Parent)).ToList();

#if DEBUG
        //Console.WriteLine("WM:" + wm + " PIVOT:" + pivotTotal + " Bonus:" + round.Bonus);
        //Console.WriteLine("From rack " + tilesFromRack + " " + round.Tiles.Where(p => p.Parent.Status == 0).Sum(p => p.Points * p.Parent.LetterMultiplier) * wm);
        //Console.WriteLine("From board " + round.Tiles.Where(p => p.Parent.Status == 1).Sum(p => p.Points) * wm);

        //round.DebugRound("Word found");
#endif
    }
}
