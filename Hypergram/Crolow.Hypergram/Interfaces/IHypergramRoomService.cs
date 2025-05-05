using Kalow.Hypergram.Logic.Models.GameSetup;

namespace MauiBlazorWeb.Shared.Interfaces.Hypergram
{
    public interface IHypergramRoomService
    {
        Task<HypergramUser> GetUser();
        Task<List<HypergramConfig>> GetConfigs();
    }
}