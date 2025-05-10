using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.Common;
using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Factories;
using Microsoft.AspNetCore.Components;

namespace Crolow.TopMachine.ComponentControls.Topping
{
    public class ToppingHomeComponent : ComponentBase, IDisposable
    {
        [Inject]
        IGameConfigService GameConfigService { get; set; }
        [Inject]
        IBoardService BoardService { get; set; }
        [Inject]
        ILetterService LetterService { get; set; }
        [Inject]
        IDictionaryService DictionaryService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }
        public List<ToppingConfigurationContainer> results = new List<ToppingConfigurationContainer>();

        protected async override void OnInitialized()
        {
            var list = GameConfigService.LoadAll();
            var boards = BoardService.LoadAll();
            var letters = LetterService.LoadAll();
            var dicos = DictionaryService.LoadAll();

            foreach (var config in list)
            {
                var item = new ToppingConfigurationContainer();
                item.GameConfig = config;
                item.LetterConfig = letters.FirstOrDefault(p => config.LetterConfig == p.Id);
                item.BoardGrid = boards.FirstOrDefault(p => config.BoardConfig == p.Id);
                item.Dictionary = dicos.FirstOrDefault(p => p.Id == item.LetterConfig?.DictionaryId);

                if (item.BoardGrid != null
                    && item.Dictionary != null
                    && item.BoardGrid != null
                    && item.LetterConfig != null)
                {
                    item.IsValid = true;
                }

                results.Add(item);
            }
        }
        protected async void StartGame(ToppingConfigurationContainer container)
        {
            var factory = new ToppingFactory();
            var game = factory.CreateGame(container, DictionaryService, FileSystem.AppDataDirectory);
            ApplicationContext.CurrentGame = game;
            Navigation.NavigateTo("/topping/playground");
        }

        public void Dispose()
        {

        }
    }
}
