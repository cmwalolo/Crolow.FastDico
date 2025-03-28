namespace Crolow.TopMachine.Data
{
    public class DatabaseSettings
    {
        public List<DatabaseSetting> Values { get; set; } = new();
    }
    public class DatabaseSetting
    {
        public DatabaseSetting()
        {

        }

        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
    }
}
