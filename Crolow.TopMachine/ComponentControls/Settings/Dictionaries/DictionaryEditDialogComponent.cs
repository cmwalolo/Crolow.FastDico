using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Crolow.TopMachine.ComponentControls.Settings.Dictionaries
{
    public class DictionaryEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }


        [Parameter]
        public IDictionaryModel Dictionary { get; set; }

        protected async override void OnInitialized()
        {
        }

        public void SaveAndClose()
        {
            DialogService.Close(Dictionary);
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
