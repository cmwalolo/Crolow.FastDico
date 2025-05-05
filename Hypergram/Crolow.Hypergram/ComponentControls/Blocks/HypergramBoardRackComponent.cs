using Kalow.Hypergram.Core.Solver.Utils;
using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.Interfaces;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorWeb.Shared.ComponentBases.Hypergram.Blocks
{
    public class HypergramBoardRackComponent : ComponentBase
    {
        [Parameter]
        public bool Highlight { get; set; }

        [Parameter]
        public HypergramWordContainer WordContainer { get; set; }

        [Parameter]
        public HypergramRoom Room { get; set; }

        [Inject]
        IMessageService messageService { get; set; }
        public HypergramBoardRackComponent()
        {
        }

        public void SelectBoardRack()
        {
            messageService.SendMessage(new Models.MessageModel()
            {
                Type = Models.MessageModel.MessageType.BoardRackSelected,
                MessageObject = WordContainer
            });
        }

        protected async override void OnInitialized()
        {

            Console.WriteLine("cpicpi");
        }
    }
}
