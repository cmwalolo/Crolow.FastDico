using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
using Crolow.TopMachine.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Settings.Dictionaries
{
    public class DictionariesComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        IDictionaryService DictionaryService { get; set; }


        public List<DictionaryModel> results = new List<DictionaryModel>();
        public RadzenDataGrid<DictionaryModel> grid;

        protected async override void OnInitialized()
        {
            results = DictionaryService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(DictionaryModel album)
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Dictionary", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is DictionaryModel)
            {
                album = result as DictionaryModel;
                album.EditState = Data.Interfaces.EditState.Update;

                DictionaryService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(DictionaryModel album)
        {
            album.EditState = Data.Interfaces.EditState.ToDelete;
            DictionaryService.Update(album);
            results.Remove(album);
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Dictionary", new DictionaryModel() }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is DictionaryModel)
            {
                var newAlbum = result as DictionaryModel;
                newAlbum.EditState = Data.Interfaces.EditState.New;

                DictionaryService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated

            }
        }

    }
}
