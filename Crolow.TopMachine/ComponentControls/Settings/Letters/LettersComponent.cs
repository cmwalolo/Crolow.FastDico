using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Letters;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
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


        public List<ILetterConfigModel> results = new List<ILetterConfigModel>();
        public List<IDictionaryModel> dicos = new List<IDictionaryModel>();
        public RadzenDataGrid<ILetterConfigModel> grid;

        protected async override void OnInitialized()
        {
            results = LetterService.LoadAll();
            dicos = DictionaryService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(ILetterConfigModel album)
        {

            var result = await DialogService.OpenAsync<LettersEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Letter", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is ILetterConfigModel)
            {
                album = result as ILetterConfigModel;
                album.EditState = EditState.Update;

                LetterService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(ILetterConfigModel album)
        {
            album.EditState = EditState.ToDelete;
            LetterService.Update(album);
            results.Remove(album);
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task CopyItem(ILetterConfigModel album)
        {
            var newConfig = new LetterConfigModel
            {
                Letters = album.Letters,
                Name = album.Name + " - copy"
            };
            newConfig.EditState = EditState.New;

            LetterService.Update(newConfig);
            results.Add(newConfig);

            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<LettersEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Letter", new LetterConfigModel() }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is ILetterConfigModel)
            {
                var newAlbum = result as ILetterConfigModel;
                newAlbum.EditState = EditState.New;

                LetterService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated
            }
        }

    }
}
