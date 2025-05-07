using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Models.ScrabbleApi;
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

        public List<GameConfigurationContainer> results = new List<GameConfigurationContainer>();

        protected async override void OnInitialized()
        {
            var list = GameConfigService.LoadAll();
            var boards = BoardService.LoadAll();
            var letters = LetterService.LoadAll();
            var dicos = DictionaryService.LoadAll();

            foreach (var config in list)
            {
                var item = new GameConfigurationContainer();
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

        public void Dispose()
        {

        }
    }
}
