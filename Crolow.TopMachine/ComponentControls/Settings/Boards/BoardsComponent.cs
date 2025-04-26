using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Dictionaries;
using Crolow.TopMachine.Core.Interfaces;
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

            var result = await DialogService.OpenAsync<DictionaryEditDialog>("Album Details", new Dictionary<string, object>
            {
                { "Dictionary", album }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is BoardGridModel)
            {
                album = result as BoardGridModel;
                album.EditState = Data.Interfaces.EditState.Update;

                BoardService.Update(album);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(BoardGridModel album)
        {
            album.EditState = Data.Interfaces.EditState.ToDelete;
            BoardService.Update(album);

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
