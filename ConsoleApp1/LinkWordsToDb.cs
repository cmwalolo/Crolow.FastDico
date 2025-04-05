using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.ScrabbleApi.Utils;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Interfaces;
using Crolow.TopMachine.Data.Repositories;
using Kalow.Apps.Common.DataTypes;
using Newtonsoft.Json;

namespace LuceneWordExtractor
{
    public static class LinkWordsToDb
    {
        public class tmpWord
        {
            public string Content { get; set; }
            public string Definition { get; set; }
            public string Mot { get; set; }

        }

        public static void Start(bool onlyNew, bool onlyMultiple, bool clean)
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
            DicoWordsToDicoDataManager<WordToDicoModel> wd = new DicoWordsToDicoDataManager<WordToDicoModel>(settings);

            dm.Repository.CreateIndex<WordEntryModel>("normalizedWord", "NormalizedWord");
            dm.Repository.CreateIndex<WordEntryModel>("word", "Word");
            wd.Repository.CreateIndex<WordToDicoModel>("word", "Word");

            int counter = 0, c = 0;
            var allWords = new List<WordToDicoModel>();
            allWords = wd.GetAllNodes().Result.OrderBy(p => p.Word).ToList();

            if (clean)
            {
                allWords.ForEach(p =>
                {
                    p.Word = p.Word.ToLower();
                    p.Parent = KalowId.Empty;
                    p.EditState = EditState.Update;
                });
                wd.UpdateAll(allWords);
            }

            var allEntries = new List<WordEntryModel>();
            allEntries = dm.GetAllNodes().Result.OrderBy(p => p.Word).Select(p => p).ToList(); ;

            StopWatcher sw = new StopWatcher(null);

            var files = Directory.GetFiles("C:\\dev\\Crolow.FastDico\\jsondico");

            foreach (var file in files)
            {
                string obj = File.ReadAllText(file);
                var list = JsonConvert.DeserializeObject<List<tmpWord>>(obj);
                foreach (var word in list.Where(p => p.Content == "Dico"))
                {
                    var search = word.Mot.ToLower();


                    var splittedWords = word.Definition.Split("---");
                    if (onlyMultiple) splittedWords = splittedWords.Skip(1).ToArray();
                    bool first = true;
                    var targets = allWords.Where(p => p.Word == search).ToList();

                    if (splittedWords.Length > 1)
                    {
                        Console.WriteLine("Multiple Words : " + search);
                    }
                    foreach (var splittedWord in splittedWords)
                    {
                        var isNew = false;
                        var def = splittedWord.Trim().Split(' ')[0];
                        def = def.Split(',')[0].ToLower();
                        var dicos = allEntries.Where(p => p.Word == def).ToList();

                        if (!dicos.Any())
                        {
                            var dico = new WordEntryModel();
                            dico.Id = KalowId.NewObjectId();
                            dico.EditState = Crolow.TopMachine.Data.Interfaces.EditState.New;
                            dico.NormalizedWord = def.NormalizeString();
                            dico.Word = def;
                            dico.Source = "external";
                            allEntries.Add(dico);
                            dicos.Add(dico);
                            isNew = true;
                        }

                        foreach (var dico in dicos)
                        {
                            if (!targets.Any(p => p.Parent == dico.Id))
                            {
                                var target = targets.Find(p => p.Parent == KalowId.Empty);
                                if (target == null)
                                {
                                    target = new WordToDicoModel
                                    {
                                        Id = KalowId.NewObjectId(),
                                        Word = search.ToLower(),
                                        EditState = Crolow.TopMachine.Data.Interfaces.EditState.New,
                                        Parent = dico.Id
                                    };

                                    allWords.Add(target);
                                    targets.Add(target);
                                }
                                else
                                {
                                    target.Parent = dico.Id;
                                    target.EditState = EditState.Update;
                                }
                            }
                        }
                    }

                    counter++;
                    if (c++ == 10000)
                    {
                        sw.Message = "Counter : " + counter;
                        sw.Dispose();
                        sw = new StopWatcher(null);
                        wd.UpdateAll(allWords);
                        c = 0;
                    }
                }
            }
            allEntries = allEntries.Where(p => p.EditState != EditState.Unchanged).ToList();
            allWords = allWords.Where(p => p.EditState != EditState.Unchanged).ToList();

            Console.WriteLine($"Updating Entries {allEntries.Count()} records ");
            Console.WriteLine($"Updating Words {allWords.Count()} records ");

            dm.UpdateAll(allEntries);
            wd.UpdateAll(allWords);
            c = 0;
        }


    }
}
