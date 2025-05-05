using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;

namespace Kalow.Hypergram.Core.Solver.Utils
{
    public class HypergramBoard
    {
        public HypergramConfig Config { get; set; }
        public HypergramBoardConfig BoardConfig { get; set; }

        public List<HypergramPlayer> PlayerBoards { get; set; } = new List<HypergramPlayer>();
        public List<HypergramWordContainer> GameRacks { get; set; } = new List<HypergramWordContainer>();
        public List<HypergramGameRound> Rounds { get; set; } = new List<HypergramGameRound> { };

        public HypergramBag GameBag { get; set; }
        public int CurrentPlayer { get; set; } = 0;

        public bool LastRound { get; set; } = false;
        public int LastPlayedRack { get; set; } = -1;
        public int TotalRounds { get; set; } = 0;

        public HypergramPlayer GetCurrentPlayer()
        {
            return PlayerBoards[CurrentPlayer];
        }
    }
}