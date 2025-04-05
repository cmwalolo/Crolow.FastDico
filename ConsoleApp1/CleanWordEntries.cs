using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Interfaces;
using Crolow.TopMachine.Data.Repositories;

namespace LuceneWordExtractor
{
    public static class CleanWordEntries
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
            var words = dm.GetAllNodes().Result.ToList();
            int c = 0;
            foreach (var word in words)
            {
                if (word.Definitions.Any())
                {
                    for (int x = 0; x < word.Definitions.Count; x++)
                    {
                        if (word.Definitions[x].CatGram.StartsWith("Forme de"))
                        {
                            word.Definitions.RemoveAt(x);
                            x--;
                            word.EditState = EditState.Update;
                        }
                    }

                    if (!word.Definitions.Any())
                    {
                        word.EditState = EditState.ToDelete;
                        c++;
                    }
                }
            }

            Console.WriteLine($"Deleting {c} records ");
            Console.WriteLine(words.Count(p => p.EditState == EditState.ToDelete));
            dm.UpdateAll(words);

        }
    }
}
