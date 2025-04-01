using Crolow.FastDico.GadDag;
using Crolow.FastDico.Models.Models.Finder;
using Crolow.FastDico.Search;
using Crolow.FastDico.Utils;
using Crolow.TopMachine.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;

namespace Crolow.TopMachine.ComponentControls.Finder
{
    public class WordFinderComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        IDictionaryService DictionaryService { get; set; }

        public string SearchPattern { get; set; }


        public ObservableCollection<FinderResult> Results = new ObservableCollection<FinderResult>();
        private GadDagCompiler gaddag;
        private GadDagSearchCore searcher;
        public RadzenDataGrid<WordResults> grid;
        public bool searchActive = false;

        protected async override void OnInitialized()
        {
            var results = DictionaryService.LoadAll();
            var currentDico = results.FirstOrDefault(p => p.IsDefault);
            if (currentDico == null)
            {
                currentDico = results.FirstOrDefault();
            }

            if (currentDico != null)
            {
                string f = FileSystem.AppDataDirectory + "\\" + currentDico.DictionaryFile;
                gaddag = new GadDagCompiler();
                gaddag.ReadFromFile(f);
                searcher = new GadDagSearchCore(gaddag.Root, 250);
            }
        }

        public void Dispose()
        {

        }

        public void Search7plus1()
        {
            Results.Clear();
            WordResults results = new WordResults();
            results.Words.AddRange(searcher.FindAllWordsFromLetters(SearchPattern, "").Words);
            results.Words.AddRange(searcher.FindAllWordsFromLetters(SearchPattern, "?").Words);
            ComposeResults(results, 1);

        }

        public void SearchSmaller()
        {
            Results.Clear();
            WordResults results = searcher.FindAllWordsSmaller(SearchPattern);
            ComposeResults(results, 2);
        }

        public void SearchGreater()
        {
            Results.Clear();
            WordResults results = searcher.FindAllWordsGreater(SearchPattern, 5);
            ComposeResults(results, 1);
        }

        public void SearchContaining()
        {
            Results.Clear();
            WordResults results = searcher.FindAllWordsContaining(SearchPattern);
            ComposeResults(results, 1);
        }

        public async void SearchMoreOrLess()
        {
            string[] p = new string[] { "", "<1>", "<2>", "-1+", ">1", "<1" };
            Results.Clear();
            WordResults results = searcher.FindAllWordsMoreOrLess(SearchPattern);

            searchActive = true;
            await InvokeAsync((Action)(() =>
            {
                List<FinderResult> final = new List<FinderResult>();

                foreach (var r in results.Words)
                {
                    var row = new FinderResult();
                    row.Word = WordTilesUtils.ConvertBytesToWordForDisplay(r).ToUpper();
                    row.Additional = p[r.Status];

                    if (string.IsNullOrEmpty(row.Additional))
                    {
                        Console.Write("pp");
                    }

                    var list = GadDagUtils.FindPlusOne(gaddag.Root, row.Word.ToLower());

                    row.Prefix = string.Join("", list.Where(p => p.Key == 0).Select(p => p.Value).ToArray()).ToUpper();
                    row.Suffix = string.Join("", list.Where(p => p.Key == 1).Select(p => p.Value).ToArray()).ToUpper();
                    final.Add(row);
                }

                final = final.OrderBy(p => p.Additional).ThenBy(p => p.Word).ToList();
                foreach (var item in final)
                {
                    Results.Add(item);
                }
                searchActive = false;
                StateHasChanged();
            }));

        }

        public void SearchWildcard()
        {
            Results.Clear();
            WordResults results = searcher.FindAllWordsWithPattern(SearchPattern);
            ComposeResults(results, 1);
        }

        public void SearchRegex()
        {
        }


        private async void ComposeResults(WordResults results, int sortType)
        {
            searchActive = true;
            await InvokeAsync((Action)(() =>
            {
                List<FinderResult> final = new List<FinderResult>();

                foreach (var r in results.Words)
                {
                    var row = new FinderResult();
                    row.Word = WordTilesUtils.ConvertBytesToWordForDisplay(r).ToUpper();
                    row.Additional = WordTilesUtils.ConvertBytesToWordByStatus(r, 1).ToUpper();

                    var list = GadDagUtils.FindPlusOne(gaddag.Root, row.Word.ToLower());

                    row.Prefix = string.Join("", list.Where(p => p.Key == 0).Select(p => p.Value).ToArray()).ToUpper();
                    row.Suffix = string.Join("", list.Where(p => p.Key == 1).Select(p => p.Value).ToArray()).ToUpper();
                    final.Add(row);
                }

                switch (sortType)
                {
                    case 1:
                        final = final.OrderBy(p => p.Additional.Length).ThenBy(p => p.Additional).ThenBy(p => p.Word).ToList();
                        break;
                    case 2:
                        final = final.OrderByDescending(p => p.Word.Length).ThenBy(p => p.Word).ToList();
                        break;
                }

                foreach (var item in final)
                {
                    Results.Add(item);
                }
                searchActive = false;
                StateHasChanged();
            }));
        }

    }
}
