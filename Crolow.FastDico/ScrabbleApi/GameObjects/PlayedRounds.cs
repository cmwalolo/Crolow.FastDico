using Crolow.Fast.Dawg.Utils;
using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.Fast.Dawg.ScrabbleApi;

public partial class ScrabbleAI
{
    public class PlayedRounds
    {
        PlayConfig Config { get; set; }
        public int MaxPoints { get; set; }
        public List<PlayedRound> Rounds { get; set; }

        public PlayedRound CurrentRound { get; set; }
        public PlayedRounds(PlayConfig config)
        {
            Config = config;
            Rounds = new List<PlayedRound>();
            CurrentRound = new PlayedRound();
        }
        public void SetRound(PlayedRound round)
        {
            int wm = 1;
            foreach (var t in round.Tiles)
            {
                wm *= t.WordMultiplier;
            }

            round.Points = round.Tiles.Sum(p => p.Points * p.LetterMultiplier) * wm;

            var tilesFromRack = round.Tiles.Count(p => p.Status == 0);
            if (tilesFromRack > 0 && tilesFromRack < Config.Bonus.Count())
            {
                round.Bonus = Config.Bonus[tilesFromRack - 1];
            }

            round.Points += round.Bonus;

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

            DebugRound(round);

#endif
        }

        public void DebugRound(PlayedRound round)
        {
            var l = round.Tiles.Take(round.Pivot).Select(p => p.Letter).ToList();
            var m = round.Tiles.Skip(round.Pivot).Select(p => p.Letter).ToList();
            if (round.Pivot != 0)
            {
                m.Reverse();
            }
            if (l.Count() > 0)
            {
                m.Add(31);
                m.AddRange(l);
            }

            string res = DawgUtils.ConvertBytesToWord(m);
            var txt = $"Word found : {res} "
                + round.Points + " : " + round.GetPosition();
            Console.WriteLine(txt);
        }
    }
}