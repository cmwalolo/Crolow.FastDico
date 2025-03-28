using System.Linq.Expressions;

namespace Crolow.Pix.Data.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task Add<T>(T item);
        Task AddBulk<T>(IEnumerable<T> items);
        void CreateIndex<T>(string name, string fields);
        Task<T> Get<T>(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAll<T>();
        Task<IEnumerable<T>> List<T>(Expression<Func<T, bool>> filter);
        Task<bool> Remove<T>(Expression<Func<T, bool>> filter);
        Task<bool> Update<T>(Expression<Func<T, bool>> filter, T item);
    }
}