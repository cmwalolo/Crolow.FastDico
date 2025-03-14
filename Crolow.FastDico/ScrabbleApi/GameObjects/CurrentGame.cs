using Crolow.FastDico.ScrabbleApi.Config;
using static Crolow.FastDico.ScrabbleApi.ScrabbleAI;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class CurrentGame
{
    public CurrentGame()
    {
        RoundsPlayed = new List<PlayedRound>();
    }

    public PlayConfiguration Configuration { get; set; }
    public int Round { get; set; }
    public List<PlayedRound> RoundsPlayed { get; set; }
    public int TotalPoints { get; set; }
    public int PlayTime { get; set; }

}
