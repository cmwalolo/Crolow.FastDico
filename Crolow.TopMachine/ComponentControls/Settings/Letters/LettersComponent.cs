using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
using Crolow.TopMachine.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Settings.Letters
{
    public class LettersComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        ILetterService LetterService { get; set; }


        public List<LetterConfigModel> results = new List<LetterConfigModel>();
        public RadzenDataGrid<LetterConfigModel> grid;

        protected async override void OnInitialized()
        {
            results = LetterService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(LetterConfigModel album)
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Dictionary", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is LetterConfigModel)
            {
                album = result as LetterConfigModel;
                album.EditState = Data.Interfaces.EditState.Update;

                LetterService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(LetterConfigModel album)
        {
            album.EditState = Data.Interfaces.EditState.ToDelete;
            LetterService.Update(album);

            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Dictionary", new DictionaryModel() }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is LetterConfigModel)
            {
                var newAlbum = result as LetterConfigModel;
                newAlbum.EditState = Data.Interfaces.EditState.New;

                LetterService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated
            }
        }

    }
}
