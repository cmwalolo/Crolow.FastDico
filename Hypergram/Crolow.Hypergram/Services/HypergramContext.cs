using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.Models;

namespace MauiBlazorWeb.Shared.Services.Hypergram
{
    public class HypergramContext
    {
        public static HypergramRoom CurrentRoom { get; set; }
        public static CurrentUser CurrentUser { get; set; }
        public static HypergramPlayer CurrentPlayer { get; set; }

    }
}
