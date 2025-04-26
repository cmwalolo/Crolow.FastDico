using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
using Crolow.TopMachine.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Settings.GameConfigs
{
    public class GameConfigsComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        IGameConfigService GameConfigService { get; set; }


        public List<GameConfigModel> results = new List<GameConfigModel>();
        public RadzenDataGrid<LetterConfigModel> grid;

        protected async override void OnInitialized()
        {
            results = GameConfigService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(GameConfigModel album)
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Dictionary", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is GameConfigModel)
            {
                album = result as GameConfigModel;
                album.EditState = Data.Interfaces.EditState.Update;

                GameConfigService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(GameConfigModel album)
        {
            album.EditState = Data.Interfaces.EditState.ToDelete;
            GameConfigService.Update(album);

            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Dictionary", new DictionaryModel() }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is GameConfigModel)
            {
                var newAlbum = result as GameConfigModel;
                newAlbum.EditState = Data.Interfaces.EditState.New;

                GameConfigService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated
            }
        }

    }
}
