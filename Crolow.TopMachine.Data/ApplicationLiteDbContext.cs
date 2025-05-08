using Crolow.TopMachine.Data;
using LiteDB;

namespace Crolow.Pix.Data
{
    public class ApplicationLiteDbContext
    {
        public static BsonMapper BsonMapper { get; set; }

        // Clients (One by Dtaabase) are Opened and remains open during LifeTime. 
        private static Dictionary<string, LiteDatabase> clients = new Dictionary<string, LiteDatabase>();
        private static object _lock = new object();

        private readonly DatabaseSetting setting = null;
        private readonly string tableName = null;
        protected LiteDatabase client;

        public static LiteDatabase GetClient(string setting)
        {
            if (!clients.ContainsKey(setting))
            {
                lock (_lock)
                {
                    if (!clients.ContainsKey(setting))
                    {
                        bool ok = clients.TryAdd(setting, new LiteDatabase(setting, BsonMapper));
                    }
                }
            }
            return clients[setting];
        }

        public ApplicationLiteDbContext(DatabaseSetting setting, string tableName)
        {
            client = GetClient(setting.ConnectionString);
            this.setting = setting;
            this.tableName = $"{setting.Schema}.{tableName}";
        }

        public string TableName
        {
            get { return tableName; }
        }
        public ILiteCollection<T> Collection<T>()
        {
            return client.GetCollection<T>(tableName.Replace('.', '_'));
        }

    }
}