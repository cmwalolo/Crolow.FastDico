using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.GameConfigs;
using Crolow.TopMachine.Core.Interfaces;
using Kalow.Apps.Common.DataTypes;
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
        [Inject]
        IBoardService BoardService { get; set; }
        [Inject]
        ILetterService LetterService { get; set; }


        public List<GameConfigModel> results = new List<GameConfigModel>();
        public List<BoardGridModel> boards = new List<BoardGridModel>();
        public List<LetterConfigModel> letters = new List<LetterConfigModel>();
        public RadzenDataGrid<GameConfigModel> grid;

        protected async override void OnInitialized()
        {
            results = GameConfigService.LoadAll();
            boards = BoardService.LoadAll();
            letters = LetterService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(GameConfigModel album)
        {

            var result = await DialogService.OpenAsync<GameConfigEditDialog>("Game Details", new Dictionary<string, object>
            {
                { "GameConfig", album }
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
            results.Remove(album);
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<GameConfigEditDialog>("Game Details", new Dictionary<string, object>
            {
                { "GameConfig", new GameConfigModel() }
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

        public async Task CopyItem(GameConfigModel album)
        {
            var newConfig = album as GameConfigModel;
            newConfig.Id = KalowId.NewObjectId();
            newConfig.EditState = Data.Interfaces.EditState.New;
            GameConfigService.Update(newConfig);
            results = GameConfigService.LoadAll();
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

    }
}
