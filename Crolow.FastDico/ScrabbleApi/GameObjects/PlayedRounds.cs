using Crolow.FastDico.Utils;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi;

public partial class ScrabbleAI
{
    public class PlayedRounds
    {
        GameConfig Config { get; set; }
        public int MaxPoints { get; set; }
        public List<PlayedRound> Rounds { get; set; }

        public PlayedRound CurrentRound { get; set; }
        public PlayedRounds(GameConfig config)
        {
            Config = config;
            Rounds = new List<PlayedRound>();
            CurrentRound = new PlayedRound();
        }
        public void SetRound(PlayedRound round)
        {
            int wm = 1;
            int pivotTotal = 0;
            foreach (var t in round.Tiles)
            {
                if (t.Status == 0)
                {
                    wm *= t.WordMultiplier;
                    if (t.Mask > 0)
                    {
                        pivotTotal += (t.Points * t.LetterMultiplier * t.WordMultiplier) + (t.Mask * t.WordMultiplier);
                    }
                }
            }

            if (wm > 2)
            {
                // Console.WriteLine("WFT" + round.Position.X + " " + round.Position.Y);
            }

            round.Points = round.Tiles.Where(p => p.Status == 0).Sum(p => p.Points * p.LetterMultiplier) * wm;
            round.Points += round.Tiles.Where(p => p.Status == 1).Sum(p => p.Points) * wm;

            var tilesFromRack = round.Tiles.Count(p => p.Status == 0);
            if (tilesFromRack > 0 && tilesFromRack < Config.Bonus.Count())
            {
                round.Bonus = Config.Bonus[tilesFromRack - 1];
            }

            round.Points += round.Bonus + pivotTotal;

            if (round.Points > MaxPoints)
            {
                Rounds.Clear();
                Rounds.Add(round);
                MaxPoints = round.Points;
            }
            else
            {
                return;
            }

            round.Tiles = round.Tiles.Select(p => new Tile(p)).ToList();

#if DEBUG
            Console.WriteLine("WM:" + wm + " PIVOT:" + pivotTotal + " Bonus:" + round.Bonus);
            Console.WriteLine("From rack " + tilesFromRack + " " + round.Tiles.Where(p => p.Status == 0).Sum(p => p.Points * p.LetterMultiplier) * wm);
            Console.WriteLine("From board " + round.Tiles.Where(p => p.Status == 1).Sum(p => p.Points) * wm);

            round.DebugRound("Word found");
#endif
        }
    }
}