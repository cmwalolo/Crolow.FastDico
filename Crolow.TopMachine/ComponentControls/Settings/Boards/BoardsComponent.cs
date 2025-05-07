using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Boards;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
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


        public List<BoardGridModel> results = new List<BoardGridModel>();
        public RadzenDataGrid<BoardGridModel> grid;

        protected async override void OnInitialized()
        {
            results = BoardService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(BoardGridModel album)
        {

            var result = await DialogService.OpenAsync<BoardsEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Board", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is BoardGridModel)
            {
                album = result as BoardGridModel;
                album.EditState = Data.Interfaces.EditState.Update;

                BoardService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task CopyItem(BoardGridModel album)
        {
            var newConfig = new BoardGridModel
            {
                Name = album.Name + " - copy",
                SizeH = album.SizeH,
                SizeV = album.SizeV,
                Configuration = album.Configuration.ToList()

            };

            newConfig.EditState = Data.Interfaces.EditState.New;
            BoardService.Update(newConfig);
            results.Add(newConfig);

            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task DeleteItem(BoardGridModel album)
        {
            album.EditState = Data.Interfaces.EditState.ToDelete;
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

            if (result != null && result is BoardGridModel)
            {
                var newAlbum = result as BoardGridModel;
                newAlbum.EditState = Data.Interfaces.EditState.New;

                BoardService.Update(newAlbum);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated
            }
        }

    }
}
