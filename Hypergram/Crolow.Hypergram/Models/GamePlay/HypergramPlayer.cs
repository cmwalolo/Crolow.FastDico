using Kalow.Hypergram.Core.Solver.Utils;
using Kalow.Hypergram.Logic.Models.GameSetup;

namespace Kalow.Hypergram.Logic.Models.GamePlay

{

    public class HypergramPlayer : HypergramUser
    {
        public bool Initialized { get; set; }
        public int TotalPoints { get; set; }
        public long TimeUsed { get; set; }
        public HypergramWordContainer LastRack { get; set; }
        public HypergramWordContainer CurrentRack { get; set; }

        public bool IsLocked { get; set; }
        public bool IsRobot { get; set; }


        public int NextPickAmount { get; set; } = 0;
        public bool IsCurrent { get; set; } = false;
        public bool IsEnabled { get; set; } = false;
        public bool CanChange { get; set; } = false;
        public bool CanPick { get; set; } = false;
        public bool CanPlay { get; set; } = false;
        public bool CanPass { get; set; } = false;
    }

}