using Crolow.FastDico.Models.ScrabbleApi.Entities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using static Crolow.FastDico.Models.ScrabbleApi.Entities.BoardGridModel;

namespace Crolow.TopMachine.ComponentControls.Settings.Boards
{
    public class BoardEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }


        [Parameter]
        public BoardGridModel Board { get; set; }
        public string Configuration { get; set; }


        protected async override void OnInitialized()
        {
            Configuration = JsonConvert.SerializeObject(Board.Configuration, Formatting.Indented);
        }

        public async void SaveAndClose()
        {
            try
            {
                Board.Configuration = JsonConvert.DeserializeObject<List<MultiplierData>>(Configuration);
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
