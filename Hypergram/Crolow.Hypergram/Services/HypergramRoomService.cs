using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.ApiServices.Models.Apis.Hypergram;
using MauiBlazorWeb.Shared.Constants;
using MauiBlazorWeb.Shared.Interfaces;
using MauiBlazorWeb.Shared.Interfaces.Hypergram;
using MauiBlazorWeb.Shared.Models;

namespace MauiBlazorWeb.Shared.Services.Hypergram
{
    public class HypergramRoomService : IHypergramRoomService
    {
        IApiFactory apiFactory;
        IStorageContainer storageService;

        public HypergramRoomService(IApiFactory apiFactory, IStorageContainer storageService)
        {
            this.apiFactory = apiFactory;
            this.storageService = storageService;

        }

        public async Task<HypergramUser> GetUser()
        {
            var value = await storageService.GetValue<CurrentUser>(StorageKeys.TopmachineToken);
            if (value != null)
            {
                return await apiFactory.CreateRequest<HypergramGetuserApi>(value).DoAPi<object, HypergramUser>(null);
            }
            return null;
        }

        public async Task<List<HypergramConfig>> GetConfigs()
        {
            var value = HypergramContext.CurrentUser;
            if (value != null)
            {
                var list = await apiFactory.CreateRequest<HypergramListConfigsApi>(value).DoAPi<object, HypergramConfig[]>(null);
                return list.ToList();
            }
            return new List<HypergramConfig>();
        }

    }
}
