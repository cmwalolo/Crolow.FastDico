using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;
using Kalow.Hypergram.Services.Interfaces;

namespace Kalow.Hypergram.Core.Services
{
    public class HypergramRoomServerService : IHypergramRoomServerService
    {
        public enum PlayerRole
        {
            Player = 0,
            Observer = 1
        }

        protected readonly IDataManager<HypergramConfig> configDatamanager;
        protected List<HypergramRoom> rooms;
        public HypergramRoomServerService(IDataManager<HypergramConfig> configDatamanager)
        {
            //this.rooms = rooms;
            this.configDatamanager = configDatamanager;
        }

        public List<HypergramRoom> GetRooms()
        {
            return rooms.GetRooms();
        }

        public void CreateConfig(HypergramConfig config)
        {
            config.EditState = EditState.New;
            configDatamanager.Update(config);
        }

        public List<HypergramConfig> GetConfigs(string language)
        {
            return configDatamanager.GetAllNodes(p => p.Language == language).OrderBy(p => p.Name).ThenBy(p => p.Name).ToList();
        }


        public HypergramRoom CreateRoom(HypergramUser user, HypergramConfig config)
        {
            var room = new HypergramRoom
            {
                Id = KalowId.NewObjectId(),
                TimeOut = DateTime.UtcNow.AddMinutes(5),
                GameStartTime = DateTime.UtcNow.AddMinutes(2),
                Owner = user,
                TotalPlayers = 1
            };


            if (!rooms.AddRoom(room))
            {
                throw new OutOfMemoryException("The room pool limit has been reached");
            }

            room.Visitors.Add(user);
            room.Board.PlayerBoards.Add(new HypergramPlayer
            {
                Id = user.Id,
                Name = user.Name
            });

            return room;
        }

        public HypergramRoom StartGame(KalowId roomId)
        {
            var room = rooms.GetRoom(roomId);
            if (room != null)
            {

            }

            return room;
        }

    }
}
