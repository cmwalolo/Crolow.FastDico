using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities.Partials;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Settings.Letters
{
    public class LettersEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }
        [Inject]
        public IDictionaryService DictionaryService { get; set; }

        [Parameter]
        public LetterConfigModel Letter { get; set; }

        public RadzenDataGrid<TileConfig> grid;
        public List<DictionaryModel> dicos;

        protected async override void OnInitialized()
        {
            dicos = DictionaryService.LoadAll();
        }

        public void SaveAndClose()
        {
            DialogService.Close(Letter);
        }

        public void CancelDialog()
        {
            DialogService.Close();
        }

        public void Dispose()
        {

        }

        DataGridEditMode editMode = DataGridEditMode.Single;
        public async Task EditRow(TileConfig tileConfig)
        {
            if (!grid.IsValid) return;
            await grid.EditRow(tileConfig);
        }

        public void OnUpdateRow(TileConfig tileConfig)
        {
        }

        public async Task SaveRow(TileConfig tileConfig)
        {
            grid.UpdateRow(tileConfig);
            grid.CancelEditRow(tileConfig);
        }

        public void CancelEdit(TileConfig tileConfig)
        {
            grid.CancelEditRow(tileConfig);
        }

        public async Task DeleteRow(TileConfig tileConfig)
        {

            if (Letter.Letters.Contains(tileConfig))
            {
                Letter.Letters.Remove(tileConfig);
                await grid.Reload();
            }
            else
            {
                grid.CancelEditRow(tileConfig);
                await grid.Reload();
            }
        }

        public async Task InsertRow()
        {
            if (!grid.IsValid) return;
            var TileConfig = new TileConfig();
            await grid.InsertRow(TileConfig);
        }

        public async Task InsertAfterRow(TileConfig row)
        {
            if (!grid.IsValid) return;
            var TileConfig = new TileConfig();
            Letter.Letters.Add(TileConfig);
            await grid.InsertAfterRow(TileConfig, row);
        }

        public void OnCreateRow(TileConfig tileConfig)
        {
        }
    }
}
