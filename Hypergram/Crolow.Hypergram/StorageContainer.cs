using MauiBlazorWeb.Shared.Interfaces;

namespace MauiBlazorWeb.Web.Services
{
    public class StorageContainer : IStorageContainer
    {
        protected readonly Blazored.LocalStorage.ILocalStorageService storage;

        public StorageContainer(Blazored.LocalStorage.ILocalStorageService storage)
        {
            this.storage = storage;
        }

        public async void SetValue<T>(string key, T value)
        {
            await storage.SetItemAsync<T>(key, value);
        }

        public async Task<T?> GetValue<T>(string key)
        {
            var value = await storage.GetItemAsync<T>(key);
            return value;
        }

        public async Task RemoveValue(string key)
        {
            await storage.RemoveItemAsync(key);
        }
    }
}
