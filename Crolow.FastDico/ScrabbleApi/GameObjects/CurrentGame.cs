using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.Fast.Dawg.ScrabbleApi;

public partial class ScrabbleAI
{
    public class CurrentGame
    {
        public PlayConfiguration Configuration { get; set; }
        public int Round { get; set; }
        public int TotalPoints { get; set; }
        public int PlayTime { get; set; }

    }
}