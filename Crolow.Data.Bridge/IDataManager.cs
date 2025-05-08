using Kalow.Apps.Common.DataTypes;
using System.Linq.Expressions;

namespace Crolow.TopMachine.Data.Bridge
{
    public interface IDataManager<T> : IDisposable where T : IDataObject
    {
        IRepository Repository { get; set; }
        Task<IEnumerable<T>> GetAllNodes();
        IEnumerable<T> GetAllNodes(Expression<Func<T, bool>> filter);
        T GetNode(Expression<Func<T, bool>> filter);
        T GetNode(KalowId dataLink);
        void Update(T data);
        void UpdateAll(IEnumerable<T> data);
    }
}