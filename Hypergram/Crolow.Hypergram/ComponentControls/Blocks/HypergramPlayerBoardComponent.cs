using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.Interfaces;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorWeb.Shared.ComponentBases.Hypergram.Blocks
{
    public class HypergramPlayerBoardComponent : ComponentBase
    {

        [Parameter]
        public HypergramPlayer Player { get; set; }

        [Parameter]
        public HypergramRoom Room { get; set; }

        [Inject]
        IMessageService messageService { get; set; }

        public HypergramPlayerBoardComponent()
        {
        }

        public void Pick()
        {
            messageService.SendMessage(new Models.MessageModel()
            {
                Type = Models.MessageModel.MessageType.Pick,
                MessageObject = Player
            });
        }
        public void RobotPlay()
        {
            messageService.SendMessage(new Models.MessageModel()
            {
                Type = Models.MessageModel.MessageType.RobotPlay,
                MessageObject = Player
            });
        }


        public void Change()
        {
            messageService.SendMessage(new Models.MessageModel()
            {
                Type = Models.MessageModel.MessageType.Change,
                MessageObject = Player
            });
        }

        public void Pass()
        {
            messageService.SendMessage(new Models.MessageModel()
            {
                Type = Models.MessageModel.MessageType.Pass,
                MessageObject = Player
            });
        }


        protected async override void OnInitialized()
        {
        }
    }
}
