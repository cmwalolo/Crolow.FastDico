using Kalow.Apps.Common.DataTypes;

namespace Kalow.Hypergram.Logic.Models.GamePlay
{
    public class HypergramGameRound
    {
        public KalowId PlayerId { get; set; }
        public KalowId TargetPlayerId { get; set; }
        public int Turn { get; set; }
        public bool HasPassed { get; set; }
        public string WordPlayed { get; set; } = string.Empty;
        public int Points { get; set; }
        public string RemainingRack { get; set; }
        public string LettersPlayed { get; set; }
        public int RackPlayed { get; set; } = -1;
        public string PreviousWord { get; set; } = string.Empty;

    }
}