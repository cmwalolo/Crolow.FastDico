using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.Interfaces;
using MauiBlazorWeb.Shared.Interfaces.Hypergram;
using MauiBlazorWeb.Shared.Interfaces.HyperGram;
using MauiBlazorWeb.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorWeb.Shared.ComponentBases.Hypergram
{
    public class HypergramRoomsComponent : ComponentBase, IDisposable
    {
        public List<HypergramConfig> ConfigList { get; set; } = new List<HypergramConfig>();
        public List<HypergramConfig> WaitingList { get; set; } = new List<HypergramConfig>();

        [Inject]
        IStorageContainer storageContainer { get; set; }

        [Inject]
        public IHypergramRoomService hypergramRoomService { get; set; }
        [Inject]
        IMessageService messageService { get; set; }
        [Inject]
        IHypergramHubService hubService { get; set; }

        [Inject]
        IHypergramGameService gameService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        public string ErrorMessage { get; set; }

        public HypergramRoomsComponent()
        {
        }
        protected async override void OnInitialized()
        {
            if (messageService != null)
            {
                messageService.OnHubMessage += MessageService_OnHubMessage;
                messageService.OnMessage += MessageService_OnMessage; ;
            }

            await InvokeAsync(async () =>
            {
                ConfigList = await hypergramRoomService.GetConfigs();
                base.OnInitialized();
                StateHasChanged();
            });
        }

        private void MessageService_OnMessage(MessageModel obj)
        {
            switch (obj.Type)
            {
                case MessageModel.MessageType.RobotPlay:
                    gameService.SetNewGame(obj.MessageObject as HypergramConfig, true);
                    navigationManager.NavigateTo("/hypergram/playfield", false);
                    break;

            }
        }

        private async void MessageService_OnHubMessage(HubMessageModel obj)
        {
            //await InvokeAsync(() =>
            //{
            //    if (obj.Type == BaseHubMessages.CreatedRoom.ToString())
            //    {
            //        var room = obj.MessageObject as TransportHypergramRoom;
            //        var oldRoom = ConfigList.FirstOrDefault(p => p.Id == room.Id);
            //        if (oldRoom != null)
            //        {
            //            ConfigList.Remove(oldRoom);
            //        }
            //        RoomList.Add(room);
            //        StateHasChanged();
            //    }
            //});
        }

        public void Dispose()
        {
            if (messageService != null)
            {
                messageService.OnHubMessage -= MessageService_OnHubMessage;
                messageService.OnMessage -= MessageService_OnMessage;
            }
        }
    }
}
