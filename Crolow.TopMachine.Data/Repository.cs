using Crolow.Pix.Data;
using Crolow.Pix.Data.Interfaces;
using LiteDB;
using System.Linq.Expressions;

namespace Kalow.Apps.DataLayer.LiteDb.Repositories
{

    internal static class Clients
    {
        public static Dictionary<string, LiteDatabase> clients = new Dictionary<string, LiteDatabase>();
    }

    public class Repository : IRepository
    {
        protected readonly DatabaseSetting databaseSetting;
        private readonly string tableName = null;
        protected LiteDatabase client;

        public Repository(DatabaseSettings settings, string db, string table)
        {
            var setting = settings.Values.FirstOrDefault(p => p.Name == db);
            this.databaseSetting = setting;
            this.tableName = table;

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


        public LiteDatabase GetClient(string connectionString)
        {
            return new LiteDatabase(connectionString);
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
                Collection<T>().Insert(item);
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
                Collection<T>().InsertBulk(items);
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

                return Collection<T>().Update(item);
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