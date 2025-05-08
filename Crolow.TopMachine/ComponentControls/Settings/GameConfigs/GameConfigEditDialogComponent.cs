using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
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

        public List<IBoardGridModel> boards = new List<IBoardGridModel>();
        public List<ILetterConfigModel> letters = new List<ILetterConfigModel>();


        public List<IDictionaryModel> Dicos { get; set; }

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
