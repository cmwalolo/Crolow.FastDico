using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Repositories;
using Kalow.Apps.Common.DataTypes;
using Newtonsoft.Json;

namespace LuceneWordExtractor
{
    public static class WiktionaryToDb
    {
        public static void Start()
        {
            DatabaseSettings settings = new DatabaseSettings();
            DatabaseSetting setting = new DatabaseSetting()
            {
                ConnectionString = "Filename=C:\\dev\\Crolow.FastDico\\Catalog\\Dictionary.db",
                Database = "Dictionary",
                Name = "Dictionary",
                Schema = "Dictionary"
            };

            settings.Values.Add(setting);

            DicoWordsDataManager<WordModel> dm = new DicoWordsDataManager<WordModel>(settings);

            var words = new List<WordModel>();
            int c = 0;
            var files = Directory.GetFiles("C:\\dev\\Crolow.FastDico\\TextFiles\\DicoDefinitions\\Wikitionnaire");
            foreach (var file in files)
            {
                string obj = File.ReadAllText(file);
                var word = JsonConvert.DeserializeObject<WordModel>(obj);
                if (word.Definitions != null && word.Definitions.Any())
                {
                    word.Id = KalowId.NewObjectId();
                    word.EditState = Crolow.TopMachine.Data.Interfaces.EditState.New;
                    words.Add(word);
                    if (c++ == 100)
                    {
                        dm.UpdateAll(words.ToArray());
                        words.Clear();
                        c = 0;
                    }
                }
            }

            if (words.Any())
            {
                dm.UpdateAll(words.ToArray());
                c = 0;
            }

        }
    }
}
