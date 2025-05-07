using Crolow.FastDico.Common.Models.Common.Entities;
using Crolow.TopMachine.Components.Pages.Settings.Users;
using Crolow.TopMachine.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Settings.Users
{
    public class UsersComponent : ComponentBase, IDisposable
    {
        [Inject]
        DialogService DialogService { get; set; }

        [Inject]
        IUserService UserService { get; set; }


        public List<User> results = new List<User>();
        public RadzenDataGrid<User> grid;

        protected async override void OnInitialized()
        {
            results = UserService.LoadAll();
        }

        public void Dispose()
        {

        }

        public async Task EditItem(User user)
        {

            var result = await DialogService.OpenAsync<UserEditDialog>("User Details", new Dictionary<string, object>
            {
                { "User", user }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is User)
            {
                user = result as User;
                user.EditState = Data.Interfaces.EditState.Update;
                UserService.Update(user);
                StateHasChanged(); // Ensure the UI is updated
            }
        }

        public async Task DeleteItem(User user)
        {
            user.EditState = Data.Interfaces.EditState.ToDelete;
            UserService.Update(user);
            results.Remove(user);
            await grid.RefreshDataAsync();
            StateHasChanged();
        }

        public async Task AddItem()
        {

            var result = await DialogService.OpenAsync<UserEditDialog>("User Details", new Dictionary<string, object>
            {
                { "User", new User() }
            }, new DialogOptions { Width = "80%", Height = "80%" });

            if (result != null && result is User)
            {
                var newUser = result as User;
                newUser.EditState = Data.Interfaces.EditState.New;
                UserService.Update(newUser);
                results.Add(newUser);
                await grid.RefreshDataAsync();
                StateHasChanged(); // Ensure the UI is updated

            }
        }

    }
}
