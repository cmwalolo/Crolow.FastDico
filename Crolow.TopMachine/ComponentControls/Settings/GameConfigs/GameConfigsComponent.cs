using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.GameConfigs;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
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


        public List<IGameConfigModel> results = new List<IGameConfigModel>();
        public List<IBoardGridModel> boards = new List<IBoardGridModel>();
        public List<ILetterConfigModel> letters = new List<ILetterConfigModel>();
        public RadzenDataGrid<IGameConfigModel> grid;

        protected async override void OnInitialized()
        {
            results = GameConfigService.LoadAll();
            boards = BoardService.LoadAll();
            letters = LetterService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(IGameConfigModel album)
        {

            var result = await DialogService.OpenAsync<GameConfigEditDialog>("Game Details", new Dictionary<string, object>
            {
                { "GameConfig", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is IGameConfigModel)
            {
                album = result as IGameConfigModel;
                album.EditState = EditState.Update;

                GameConfigService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(IGameConfigModel album)
        {
            album.EditState = EditState.ToDelete;
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

            if (result != null && result is IGameConfigModel)
            {
                var newAlbum = result as IGameConfigModel;
                newAlbum.EditState = EditState.New;

                GameConfigService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task CopyItem(IGameConfigModel album)
        {
            var newConfig = album as IGameConfigModel;
            newConfig.Id = KalowId.NewObjectId();
            newConfig.EditState = EditState.New;
            GameConfigService.Update(newConfig);
            results = GameConfigService.LoadAll();
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

    }
}
