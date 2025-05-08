using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Bridge;
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

            DicoWordsDataManager<WordEntryModel> dm = new DicoWordsDataManager<WordEntryModel>(settings);

            var words = new List<WordEntryModel>();
            int c = 0;
            var files = Directory.GetFiles("C:\\dev\\Crolow.FastDico\\TextFiles\\DicoDefinitions\\Wikitionnaire");
            foreach (var file in files)
            {
                string obj = File.ReadAllText(file);
                var word = JsonConvert.DeserializeObject<WordEntryModel>(obj);
                if (word.Definitions != null && word.Definitions.Any())
                {
                    word.Id = KalowId.NewObjectId();
                    word.EditState = EditState.New;
                    words.Add(word);
                    if (c++ == 100)
                    {
                        dm.UpdateAll(words);
                        words.Clear();
                        c = 0;
                    }
                }
            }

            if (words.Any())
            {
                dm.UpdateAll(words);
                c = 0;
            }

        }
    }
}
