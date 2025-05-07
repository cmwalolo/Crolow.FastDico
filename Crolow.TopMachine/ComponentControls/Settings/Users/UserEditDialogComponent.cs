using Crolow.FastDico.Common.Models.Common.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Crolow.TopMachine.ComponentControls.Settings.Users
{
    public partial class UserEditDialogComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }


        [Parameter]
        public User User { get; set; }

        protected async override void OnInitialized()
        {
        }

        public void SaveAndClose()
        {
            DialogService.Close(User);
        }

        public void CancelDialog()
        {
            DialogService.Close();
        }

        public void Dispose()
        {

        }
    }
}
