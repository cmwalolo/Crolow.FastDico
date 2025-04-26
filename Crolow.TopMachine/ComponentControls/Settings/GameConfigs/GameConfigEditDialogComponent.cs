using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Crolow.TopMachine.ComponentControls.Settings.Boards
{
    public class GameConfigEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }


        [Parameter]
        public GameConfigModel GameConfig { get; set; }

        protected async override void OnInitialized()
        {
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
