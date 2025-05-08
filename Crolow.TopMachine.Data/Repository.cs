using Crolow.TopMachine.Data.Bridge;
using LiteDB;
using System.Linq.Expressions;

namespace Crolow.TopMachine.Data
{

    internal static class Clients
    {
        public static Dictionary<string, LiteDatabase> clients = new Dictionary<string, LiteDatabase>();
    }

    public class Repository : IRepository
    {
        public static BsonMapper BsonMapper { get; set; }

        protected readonly DatabaseSetting databaseSetting;
        private readonly string tableName = null;
        protected LiteDatabase client;

        public Repository(DatabaseSettings settings, string db, string table)
        {
            var setting = settings.Values.FirstOrDefault(p => p.Name == db);
            databaseSetting = setting;
            tableName = table;

            if (setting != null)
            {
                if (Clients.clients.ContainsKey(setting.ConnectionString))
                {
                    client = Clients.clients[setting.ConnectionString];
                }
                else
                {
                    client = GetClient(setting.ConnectionString);
                    Clients.clients.Add(setting.ConnectionString, client);
                }
            }
        }

        public void CreateIndex<T>(string name, string fields)
        {
            Collection<T>().EnsureIndex(name, fields);
        }

        public void DropIndex<T>(string name)
        {
            Collection<T>().DropIndex(name);
        }

        public LiteDatabase GetClient(string connectionString)
        {
            return new LiteDatabase(connectionString, BsonMapper);
        }


        public async Task<IEnumerable<T>> GetAll<T>()
        {
            try
            {
                return Collection<T>().FindAll();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> filter)
        {
            try
            {
                return Collection<T>()
                                .FindOne(filter);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw;
            }
        }

        public async Task<IEnumerable<T>> List<T>(Expression<Func<T, bool>> filter)
        {
            try
            {
                return Collection<T>().Find(filter);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw;
            }
        }

        public async Task Add<T>(T item)
        {
            try
            {
                GetDynamicCollection(item).Insert(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw;
            }
        }

        public async Task AddBulk<T>(IEnumerable<T> items)
        {
            try
            {
                if (items.Any())
                {
                    GetDynamicCollection(items.First()).InsertBulk(items.Select(p => p as object));
                }
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw;
            }
        }

        public async Task<bool> Remove<T>(Expression<Func<T, bool>> filter)
        {
            try
            {
                return Collection<T>().DeleteMany(filter) > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw;
            }
        }
        public async Task<bool> Update<T>(Expression<Func<T, bool>> filter, T item)
        {
            try
            {

                return GetDynamicCollection(item).Update(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw;
            }
        }

        public ILiteCollection<T> Collection<T>()
        {
            return client.GetCollection<T>(tableName);
        }

        /// <summary>
        /// This one is used due to a bug in LiteDB
        /// The Mappers are running in a stack overflow 
        /// on serialization.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public ILiteCollection<object> GetDynamicCollection(object target)
        {
            var type = target.GetType();
            var method = typeof(LiteDatabase).GetMethod("GetCollection").MakeGenericMethod(type);
            return (ILiteCollection<object>)method.Invoke(client, new object[] { tableName, BsonAutoId.ObjectId });
        }

        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}