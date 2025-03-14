using Crolow.FastDico.ScrabbleApi.Config;
using static Crolow.FastDico.ScrabbleApi.ScrabbleAI;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class CurrentGame
{
    public CurrentGame()
    {
        RoundsPlayed = new List<PlayedRounds>();
    }

    public PlayConfiguration Configuration { get; set; }
    public int Round { get; set; }
    public List<PlayedRounds> RoundsPlayed { get; set; }
    public int TotalPoints { get; set; }
    public int PlayTime { get; set; }

}
