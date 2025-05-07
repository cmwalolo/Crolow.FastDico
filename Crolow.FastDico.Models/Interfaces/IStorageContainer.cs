namespace Crolow.FastDico.Common.Interfaces
{
    public interface IStorageContainer
    {
        Task<T?> GetValue<T>(string key);
        void SetValue<T>(string key, T value);
        Task RemoveValue(string key);
    }
}