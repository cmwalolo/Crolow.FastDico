using Crolow.TopMachine.Data.Bridge;
using Kalow.Apps.Common.DataTypes;
using LiteDB;
using System.Linq.Expressions;

namespace Crolow.TopMachine.Data
{
    public class DataManager<T> : IDataManager<T> where T : IDataObject
    {
        public IRepository Repository { get; set; }
        public DataManager(DatabaseSettings settings, string db, string table)
        {
            Repository = new Repository(settings, db, table);
        }

        public void Dispose()
        {
            if (Repository != null)
            {
                Repository.Dispose();
                Repository = null;
            }
        }

        public async Task<IEnumerable<T>> GetAllNodes()
        {
            return await Repository.GetAll<T>();
        }

        public IEnumerable<T> GetAllNodes(Expression<Func<T, bool>> filter)
        {
            return Repository.List(filter).Result;
        }

        public T GetNode(KalowId dataLink)
        {
            return Repository.Get<T>(t => t.Id == dataLink).Result;
        }

        public T GetNode(Expression<Func<T, bool>> filter)
        {
            return Repository.Get(filter).Result;
        }

        public void Update(T data)
        {
            switch (data.EditState)
            {
                case EditState.New:
                    Repository.Add(data);
                    break;
                case EditState.Update:
                    Repository.Update(t => t.Id == data.Id, data);
                    break;
                case EditState.ToDelete:
                    Repository.Remove<T>(t => t.Id == data.Id);
                    break;

            }
            data.EditState = EditState.Unchanged;
        }

        public void UpdateAll(IEnumerable<T> dataArray)
        {
            if (dataArray.Any())
            {
                // First NEW things in bulk
                // ***TODO*** Update in bulk -- Delete not possible in liteDb
                var newData = dataArray.Where(p => p.EditState == EditState.New).ToList();
                Repository.AddBulk(newData);

                foreach (var data in dataArray)
                {
                    switch (data.EditState)
                    {
                        case EditState.Update:
                            Repository.Update(t => t.Id == data.Id, data);
                            break;
                        case EditState.ToDelete:
                            Repository.Remove<T>(t => t.Id == data.Id);
                            break;

                    }
                    data.EditState = EditState.Unchanged;
                }
            }
        }
    }
}