using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using LiteDB;
using System.Linq.Expressions;

namespace Crolow.TopMachine.Data
{
    public interface IDataManager<T> : IDisposable where T : IDataObject
    {
        IEnumerable<T> GetAllNodes();
        IEnumerable<T> GetAllNodes(Expression<Func<T, bool>> filter);
        T GetNode(Expression<Func<T, bool>> filter);
        T GetNode(KalowId dataLink);
        void Update(T data);
        void UpdateAll(T[] data);
    }
    public class DataManager<T> : IDataManager<T> where T : IDataObject
    {
        protected IRepository repository;
        public DataManager(DatabaseSettings settings, string db, string table)
        {
            repository = new Repository(settings, db, table);
        }

        public void Dispose()
        {
            if (repository != null)
            {
                repository.Dispose();
                repository = null;
            }
        }

        public IEnumerable<T> GetAllNodes()
        {
            return repository.GetAll<T>().Result;
        }
        public IEnumerable<T> GetAllNodes(Expression<Func<T, bool>> filter)
        {
            return repository.List(filter).Result;
        }

        public T GetNode(KalowId dataLink)
        {
            return repository.Get<T>(t => t.Id == dataLink).Result;
        }

        public T GetNode(Expression<Func<T, bool>> filter)
        {
            return repository.Get(filter).Result;
        }

        public void Update(T data)
        {
            switch (data.EditState)
            {
                case EditState.New:
                    repository.Add(data);
                    break;
                case EditState.Update:
                    repository.Update(t => t.Id == data.Id, data);
                    break;
                case EditState.ToDelete:
                    repository.Remove<T>(t => t.Id == data.Id);
                    break;

            }
            data.EditState = EditState.Unchanged;
        }

        public void UpdateAll(T[] dataArray)
        {
            if (dataArray.Any())
            {
                // First NEW things in bulk
                // ***TODO*** Update in bulk -- Delete not possible in liteDb
                var newData = dataArray.Where(p => p.EditState == EditState.New).ToList();
                repository.AddBulk(newData);

                foreach (var data in dataArray)
                {
                    switch (data.EditState)
                    {
                        case EditState.Update:
                            repository.Update(t => t.Id == data.Id, data);
                            break;
                        case EditState.ToDelete:
                            repository.Remove<T>(t => t.Id == data.Id);
                            break;

                    }
                    data.EditState = EditState.Unchanged;
                }
            }
        }
    }
}