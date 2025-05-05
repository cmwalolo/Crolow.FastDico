using Kalow.Apps.Models.Data;
using Kalow.Hypergram.Core.Solver.Utils;
using System.Runtime.Serialization;

namespace Kalow.Hypergram.Logic.Models.GameSetup
{
    public class HypergramRoom : DataObject
    {
        public enum RoomStatus
        {
            [EnumMember]
            Empty = 0,

            [EnumMember]
            Created = 1,

            [EnumMember]
            WaitingForGame = 2,

            [EnumMember]
            WaitingForStart = 3,

            [EnumMember]
            Started = 4,

            [EnumMember]
            Finished = 5

        }

        public bool IsRobotGame { get; set; }
        public DateTime TimeStamp { get; set; }
        public RoomStatus GameStatus { get; set; }
        public DateTime TimeOut { get; set; } = new DateTime();
        public int TotalPlayers { get; set; }
        public DateTime GameStartTime { get; set; }
        public HypergramUser Owner { get; set; }
        public HypergramBoard Board { get; set; } = new HypergramBoard();
        public List<HypergramUser> Visitors { get; set; } = new List<HypergramUser> { };


    }
}