using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Repositories;
using Kalow.Apps.Common.DataTypes;

namespace LuceneWordExtractor
{
    public static class ImportAllWords
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

            DicoWordsToDicoDataManager<WordToDicoModel> dm = new DicoWordsToDicoDataManager<WordToDicoModel>(settings);

            var words = new List<WordToDicoModel>();
            int c = 0;
            var files = File.ReadLines("C:\\dev\\Crolow.FastDico\\TextFiles\\ods9-complet.txt");

            foreach (var file in files)
            {

                var word = new WordToDicoModel();
                word.Id = KalowId.NewObjectId();
                word.EditState = EditState.New;
                word.Word = file.ToLower();
                words.Add(word);
                if (c++ == 1000)
                {
                    dm.UpdateAll(words);
                    words.Clear();
                    c = 0;
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
