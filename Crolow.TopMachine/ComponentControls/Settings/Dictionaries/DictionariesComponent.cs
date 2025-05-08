using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
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


        public List<IDictionaryModel> results = new List<IDictionaryModel>();
        public RadzenDataGrid<IDictionaryModel> grid;

        protected async override void OnInitialized()
        {
            results = DictionaryService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(IDictionaryModel album)
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Dictionary Details", new Dictionary<string, object>
            {
                { "Dictionary", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is IDictionaryModel)
            {
                album = result as IDictionaryModel;
                album.EditState = EditState.Update;

                DictionaryService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(IDictionaryModel album)
        {
            album.EditState = EditState.ToDelete;
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

            if (result != null && result is IDictionaryModel)
            {
                var newAlbum = result as IDictionaryModel;
                newAlbum.EditState = EditState.New;

                DictionaryService.Update(newAlbum);
                results.Add(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated

            }
        }

    }
}
