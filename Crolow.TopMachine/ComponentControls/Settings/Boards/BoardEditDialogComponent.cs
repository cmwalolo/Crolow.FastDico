using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Crolow.TopMachine.ComponentControls.Settings.Boards
{
    public class BoardEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }


        [Parameter]
        public BoardGridModel Board { get; set; }

        protected async override void OnInitialized()
        {
        }

        public void SaveAndClose()
        {
            DialogService.Close(Board);
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
