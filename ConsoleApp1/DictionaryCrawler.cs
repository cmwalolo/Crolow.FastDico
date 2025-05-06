using Crolow.FastDico.Models.Dictionary.Entities;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Interfaces;
using Crolow.TopMachine.Data.Repositories;
using Kalow.Apps.ApiTester.Parsers;
using System.ComponentModel;

namespace Kalow.Apps.ApiTester
{
    public partial class DictionaryCrawler
    {
        public class DiscoverDictionary
        {
            public string Culture { get; set; }
            public string Word { get; set; }
            public string OriginalWord { get; set; }
            public string CatGram { get; set; }
            public bool IsEntry { get; set; }
        }

        public class AvailableSite
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public bool Normalize { get; set; }

        }


        private List<string> stringList;
        private DiscoverDictionary[] results;
        public AvailableSite CurrentSite { get; set; }

        public List<AvailableSite> AvailableSites { get; set; } =
            new List<AvailableSite>()
            {
                new AvailableSite {Name="Petit Robert", Url = "https://dictionnaire.lerobert.com/definition/{0}", Normalize = true },
                new AvailableSite {Name="Le Dictionnaire", Url = "https://www.le-dictionnaire.com/definition/{0}", Normalize = true },
                new AvailableSite {Name="Wikitionnaire", Url = "https://fr.wiktionary.org/wiki/{0}", Normalize  = false },
                new AvailableSite {Name="CNRTL", Url = "https://www.cnrtl.fr/definition/{0}" },
                new AvailableSite {Name="Larousse", Url = "https://www.larousse.fr/dictionnaires/francais/{0}" }
            };

        public DictionaryCrawler()
        {
        }

        public void Start(string site, bool onlyNew)
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

            CurrentSite = AvailableSites.Find(p => p.Name == site);
            if (CurrentSite != null)
            {
                DicoWordsDataManager<WordEntryModel> dm = new DicoWordsDataManager<WordEntryModel>(settings);
                var list = dm.GetAllNodes(p => !onlyNew || p.Source == "external").ToList();
                Crawl(list, dm);
            }
        }

        public void Crawl(List<WordEntryModel> list, DicoWordsDataManager<WordEntryModel> dm)
        {
            var site = CurrentSite;

            using (HttpClient client = new HttpClient())
            {

                int c = 0;
                foreach (var item in list)
                {
                    string word = !site.Normalize ? item.Word : item.Word.NormalizeString();

                    if (System.IO.Directory.GetFiles($"C:\\dev\\Crolow.FastDico\\TextFiles\\DicoCrawling\\{CurrentSite.Name}\\", $"{item.Word}-*").Length == 0)
                    {
                        bool retry = true;
                        while (retry)
                        {
                            retry = false;
                            string url = string.Format(site.Url, word);
                            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                            HttpResponseMessage response = client.SendAsync(request).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                string result = response.Content.ReadAsStringAsync().Result;
                                System.IO.File.WriteAllText($"C:\\dev\\Crolow.FastDico\\TextFiles\\DicoCrawling\\{CurrentSite.Name}\\{item.Word}-{c}.html", result);

                                var lastWord = new WordEntryModel();
                                switch (CurrentSite.Name)
                                {
                                    case "Wikitionnaire":
                                        lastWord = new WikitionnaireParser().Parse(item.Word, result, CurrentSite);
                                        if (lastWord.Definitions.Any())
                                        {
                                            lastWord.Id = item.Id;
                                            lastWord.Source += " 1";
                                            lastWord.EditState = EditState.Update;
                                            dm.Update(lastWord);
                                        }
                                        break;
                                }

                                System.IO.File.WriteAllText($"C:\\dev\\Crolow.FastDico\\TextFiles\\DicoCrawling\\{CurrentSite.Name}\\{item.Word}.json", lastWord == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(lastWord));
                            }
                            else
                            {
                                if (response.ReasonPhrase.Equals("Too Many Requests"))
                                {
                                    System.Threading.Thread.Sleep(15000);
                                    retry = true;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void ParsingData_DoWork(object sender, DoWorkEventArgs e)
        {
            var site = CurrentSite;

            int c = 0;
            foreach (var item in results)
            {
                string word = site.Normalize || !item.IsEntry ? item.Word : item.OriginalWord;


                if (System.IO.Directory.GetFiles($"C:\\dev\\Kalow Framework\\TextFiles\\DicoDefinitions\\{CurrentSite.Name}", $"{item.Word}-*").Count() > 0)
                {
                    continue;
                }

                var files = System.IO.Directory.GetFiles($"C:\\dev\\Kalow Framework\\TextFiles\\DicoCrawling\\{CurrentSite.Name}", $"{item.Word}-*");
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        //                        if (file.Contains("ingenuite"))
                        {
                            var lastWord = new WordEntryModel();
                            string html = System.IO.File.ReadAllText(file);
                            switch (CurrentSite.Name)
                            {
                                case "Wikitionnaire":
                                    lastWord = new WikitionnaireParser().Parse(item.OriginalWord, html, CurrentSite);
                                    break;
                            }

                            System.IO.File.WriteAllText($"C:\\dev\\Kalow Framework\\TextFiles\\DicoDefinitions\\{CurrentSite.Name}\\{System.IO.Path.GetFileNameWithoutExtension(file)}.json", lastWord == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(lastWord));
                        }
                    }
                }
            }
        }
    }
}
