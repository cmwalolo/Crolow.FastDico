using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Letters;
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
        [Inject]
        IDictionaryService DictionaryService { get; set; }


        public List<LetterConfigModel> results = new List<LetterConfigModel>();
        public List<DictionaryModel> dicos = new List<DictionaryModel>();
        public RadzenDataGrid<LetterConfigModel> grid;

        protected async override void OnInitialized()
        {
            results = LetterService.LoadAll();
            dicos = DictionaryService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(LetterConfigModel album)
        {

            var result = await DialogService.OpenAsync<LettersEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Letter", album }
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
            results.Remove(album);
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task CopyItem(LetterConfigModel album)
        {
            var newConfig = new LetterConfigModel
            {
                Letters = album.Letters,
                Name = album.Name + " - copy"
            };
            newConfig.EditState = Data.Interfaces.EditState.New;

            LetterService.Update(newConfig);
            results.Add(newConfig);

            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<LettersEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Letter", new DictionaryModel() }
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
