using Kalow.Apps.Models.Data;

namespace Kalow.Hypergram.Logic.Models.GameSetup
{
    public class HypergramConfig : DataObject
    {
        public string Name { get; set; } = "Partie standard";
        public string Language { get; set; } = "FR";
        public string Category { get; set; } = "Difficile";

        // Board configuration 
        public int BoardLength { get; set; } = 15;
        public int NumberOfRacks { get; set; } = 10;
        public int NumberOfBags { get; set; } = 3;
        public int StartRackLength { get; set; } = 3;

        // Player bag Configuration
        public int StartPlayerRackLength { get; set; } = 6;
        public int MaxPlayerRackLength { get; set; } = 6;
        public int MaxLetterPlayed { get; set; } = 6;
        public int MaxPickupLength { get; set; } = 6;
        public int TurnOver { get; set; } = 10;
        public int PickupBonus { get; set; } = 1;
        public bool PickupBonusSkip { get; set; } = false;

        // Game Configuration
        public int Seconds { get; set; } = 10;

        // Still need to be defined
        public int GameType { get; set; } = 0;

        // Still need to be defined
        public int AllowErrorType { get; set; } = 0;
        public int MinimumPlayers { get; set; } = 2;
        public int MaximumPlayers { get; set; } = 4;
        public int MultiplyTurns { get; set; } = 2;
    }
}