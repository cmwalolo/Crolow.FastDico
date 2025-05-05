using Kalow.Apps.Common.DataTypes;
using Kalow.Hypergram.Logic.Models.GameSetup;

namespace Kalow.Hypergram.Services.Interfaces
{
    public interface IHypergramRoomServerService
    {
        HypergramRoom CreateRoom(HypergramUser user, HypergramConfig config);
        HypergramRoom StartGame(KalowId roomId);
        List<HypergramRoom> GetRooms();

        void CreateConfig(HypergramConfig config);
        List<HypergramConfig> GetConfigs(string language);

    }
}