using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Boards;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Settings.Boards
{
    public class BoardsComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        IBoardService BoardService { get; set; }


        public List<IBoardGridModel> results = new List<IBoardGridModel>();
        public RadzenDataGrid<IBoardGridModel> grid;

        protected async override void OnInitialized()
        {
            results = BoardService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(IBoardGridModel album)
        {

            var result = await DialogService.OpenAsync<BoardsEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Board", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is IBoardGridModel)
            {
                album = result as IBoardGridModel;
                album.EditState = EditState.Update;

                BoardService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task CopyItem(IBoardGridModel album)
        {
            var newConfig = new BoardGridModel
            {
                Name = album.Name + " - copy",
                SizeH = album.SizeH,
                SizeV = album.SizeV,
                Configuration = album.Configuration.ToList()

            };

            newConfig.EditState = EditState.New;
            BoardService.Update(newConfig);
            results.Add(newConfig);

            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task DeleteItem(IBoardGridModel album)
        {
            album.EditState = EditState.ToDelete;
            BoardService.Update(album);
            results.Remove(album);
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Dictionary", new DictionaryModel() }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is IBoardGridModel)
            {
                var newAlbum = result as IBoardGridModel;
                newAlbum.EditState = EditState.New;

                BoardService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated
            }
        }

    }
}
