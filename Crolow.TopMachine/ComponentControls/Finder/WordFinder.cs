using Crolow.FastDico.GadDag;
using Crolow.FastDico.Models.Models.Finder;
using Crolow.FastDico.Search;
using Crolow.FastDico.Utils;
using Crolow.TopMachine.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
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
                searcher = new GadDagSearchCore(gaddag.Root);
            }
        }

        public void Dispose()
        {

        }

        public void Search7plus1()
        {
            Results.Clear();

            List<FinderResult> final = new List<FinderResult>();
            WordResults results = new WordResults();
            results.Words.AddRange(searcher.FindAllWordsFromLetters(SearchPattern, "").Words);
            results.Words.AddRange(searcher.FindAllWordsFromLetters(SearchPattern, "?").Words);

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

            foreach (var item in final.OrderBy(p => p.Additional))
            {
                Results.Add(item);
            }

            StateHasChanged(); // Ensure the UI is updated
        }

        public void SearchSmaller()
        {
            Results.Clear();

            List<FinderResult> final = new List<FinderResult>();
            WordResults results = new WordResults();
            results.Words.AddRange(searcher.FindAllWordsSmaller(SearchPattern).Words);

            foreach (var r in results.Words)
            {
                var row = new FinderResult();
                row.Word = WordTilesUtils.ConvertBytesToWordForDisplay(r).ToUpper();

                var list = GadDagUtils.FindPlusOne(gaddag.Root, row.Word.ToLower());

                row.Prefix = string.Join("", list.Where(p => p.Key == 0).Select(p => p.Value).ToArray()).ToUpper();
                row.Suffix = string.Join("", list.Where(p => p.Key == 1).Select(p => p.Value).ToArray()).ToUpper();
                final.Add(row);
            }

            foreach (var item in final.OrderBy(p => p.Additional))
            {
                Results.Add(item);
            }

            StateHasChanged(); // Ensure the UI is updated
        }

        public void SearchGreater()
        {
            Results.Clear();

            List<FinderResult> final = new List<FinderResult>();
            WordResults results = new WordResults();
            results.Words.AddRange(searcher.FindAllWordsGreater(SearchPattern, 5).Words);

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

            foreach (var item in final.OrderBy(p => p.Additional))
            {
                Results.Add(item);
            }

            StateHasChanged(); // Ensure the UI is updated
        }

        public void SearchContaining()
        {
        }

        public void SearchMoreOrLess()
        {
        }

        public void SearchWildcard()
        {
        }

        public void SearchRegex()
        {
        }


    }
}
