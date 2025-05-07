using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Crolow.TopMachine.ComponentControls.Settings.GameConfigs
{
    public class GameConfigEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        IBoardService BoardService { get; set; }
        [Inject]
        ILetterService LetterService { get; set; }

        [Parameter]
        public GameConfigModel GameConfig { get; set; }

        public List<BoardGridModel> boards = new List<BoardGridModel>();
        public List<LetterConfigModel> letters = new List<LetterConfigModel>();


        public List<DictionaryModel> Dicos { get; set; }

        protected async override void OnInitialized()
        {
            boards = BoardService.LoadAll();
            letters = LetterService.LoadAll();
        }

        public void SaveAndClose()
        {
            DialogService.Close(GameConfig);
        }

        public void CancelDialog()
        {
            DialogService.Close();
        }

        public void Dispose()
        {

        }
    }
}
