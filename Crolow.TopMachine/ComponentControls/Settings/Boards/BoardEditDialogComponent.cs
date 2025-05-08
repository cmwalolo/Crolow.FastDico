using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;

namespace Crolow.TopMachine.ComponentControls.Settings.Boards
{
    public class BoardEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }


        [Parameter]
        public IBoardGridModel Board { get; set; }
        public string Configuration { get; set; }


        protected async override void OnInitialized()
        {
            Configuration = JsonConvert.SerializeObject(Board.Configuration, Formatting.Indented);
        }

        public async void SaveAndClose()
        {
            try
            {
                Board.Configuration = JsonConvert.DeserializeObject<List<IMultiplierData>>(Configuration);
                DialogService.Close(Board);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Title", "This is a message.", "OK", "Cancel");
            }
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
