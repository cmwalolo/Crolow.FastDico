using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.Interfaces;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorWeb.Shared.ComponentBases.Hypergram.Blocks
{
    public class HypergramConfigCardComponent : ComponentBase
    {

        public string[] GameTypes = { "Classique" };

        [Parameter]
        public HypergramConfig Config { get; set; }

        [Inject]
        IMessageService messageService { get; set; }
        public HypergramConfigCardComponent()
        {
        }

        public async Task RobotPlay()
        {
            await InvokeAsync(async () =>
            {
                messageService.SendMessage(new Models.MessageModel()
                {
                    Type = Models.MessageModel.MessageType.RobotPlay,
                    MessageObject = Config
                });
            });
        }

        protected async override void OnInitialized()
        {
            Console.WriteLine("cpicpi");
        }
    }
}
