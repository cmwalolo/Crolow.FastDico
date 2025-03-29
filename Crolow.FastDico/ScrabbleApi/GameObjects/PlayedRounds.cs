using Crolow.FastDico.Utils;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;

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
                wm *= t.WordMultiplier;

                if (t.Status == 0 && t.Mask > 0)
                {
                    pivotTotal += (t.Points * t.LetterMultiplier * t.WordMultiplier) + (t.Mask * t.WordMultiplier);
                }
            }

            round.Points = round.Tiles.Sum(p => p.Points * p.LetterMultiplier) * wm;

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

#if DEBUG
            //   round.DebugRound("Word found");
#endif
        }
    }
}