using Kalow.Hypergram.Core.Solver.Utils;
using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.Interfaces;
using MauiBlazorWeb.Shared.Interfaces.HyperGram;
using MauiBlazorWeb.Shared.Models;
using MauiBlazorWeb.Shared.Services.Hypergram;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorWeb.Shared.ComponentBases.Hypergram
{
    public class HypergramPlayyFieldComponent : ComponentBase, IDisposable
    {
        [Inject]
        public IHypergramGameService hypergramGameService { get; set; }
        [Inject]
        IMessageService messageService { get; set; }
        [Inject]
        IHypergramHubService hubService { get; set; }


        public HypergramRoom CurrentRoom { get; set; }
        public HypergramGameRound LastRound { get; set; }

        public WordPlayComponent WordPlayModal { get; set; }

        public HypergramPlayer SelectedPlayer { get; set; }
        public HypergramWordContainer SelectedBoardRack { get; set; }

        public string ErrorMessage { get; set; }

        public HypergramPlayyFieldComponent()
        {
        }
        protected async override void OnInitialized()
        {
            CurrentRoom = HypergramContext.CurrentRoom;

            if (messageService != null)
            {
                messageService.OnHubMessage += MessageService_OnHubMessage;
                messageService.OnMessage += MessageService_OnMessage; ;
            }
        }

        private async void MessageService_OnMessage(MessageModel obj)
        {
            var ndx = 0;
            await InvokeAsync((Action)(() =>
            {
                switch (obj.Type)
                {
                    case MessageModel.MessageType.BoardRackSelected:
                        {
                            if (SelectedPlayer != null)
                            {
                                SelectedBoardRack = obj.MessageObject as HypergramWordContainer;
                                ndx = CurrentRoom.Board.GameRacks.FindIndex(p => p.GetWord().Equals(SelectedBoardRack.GetWord()));
                                WordPlayModal.SetWordContainers(ndx, SelectedBoardRack, SelectedPlayer.CurrentRack);
                                WordPlayModal.Open();
                                StateHasChanged();
                            }
                            break;
                        }
                    case MessageModel.MessageType.BoardRackSelectNext:
                        {
                            SelectedBoardRack = obj.MessageObject as HypergramWordContainer;
                            var racks = CurrentRoom.Board.GameRacks;
                            int len = racks.Count();

                            ndx = racks.FindIndex(p => p.GetWord().Equals(SelectedBoardRack.GetWord())) + 1;
                            if (ndx != 0)
                            {
                                if (ndx >= len)
                                {
                                    ndx = 0;
                                }

                                SelectedBoardRack = racks[ndx];
                                WordPlayModal.SetWordContainers(ndx, SelectedBoardRack, SelectedPlayer.CurrentRack);
                                StateHasChanged();
                            }
                        }
                        break;
                    case MessageModel.MessageType.BoardRackSelectPrevious:
                        {
                            SelectedBoardRack = obj.MessageObject as HypergramWordContainer;
                            var racks = CurrentRoom.Board.GameRacks;
                            int len = racks.Count();

                            ndx = racks.FindIndex(p => p.GetWord().Equals(SelectedBoardRack.GetWord())) - 1;
                            if (ndx < 0)
                            {
                                ndx = racks.Count() - 1;
                            }

                            SelectedBoardRack = racks[ndx];
                            WordPlayModal.SetWordContainers(ndx, SelectedBoardRack, SelectedPlayer.CurrentRack);
                            StateHasChanged();
                        }
                        break;

                    case MessageModel.MessageType.GameStarted:
                        hypergramGameService.SetNextPlayer();
                        StateHasChanged();
                        break;
                    case MessageModel.MessageType.RobotPlay:
                        {
                            var player = obj.MessageObject as HypergramPlayer;
                            var currentPlayer = CurrentRoom.Board.PlayerBoards[CurrentRoom.Board.CurrentPlayer];
                            var checkStolenRack = player?.Id != currentPlayer.Id;

                            LastRound = hypergramGameService.PlayNextRound(player, checkStolenRack);

                            if (checkStolenRack && LastRound.HasPassed)
                            {
                                LastRound = hypergramGameService.PlayNextRound(currentPlayer, false);
                            }

                            // Even if the round is correct. The player status needs to be reviewed.
                            if (LastRound.HasPassed)
                            {
                                var round = hypergramGameService.Pass();
                            }

                            messageService.SendMessage(new MessageModel()
                            {
                                Type = MessageModel.MessageType.RoundIsPlayed,
                                MessageObject = LastRound
                            });

                            StateHasChanged();
                        }
                        break;
                    case MessageModel.MessageType.Pick:
                        {
                            SelectedPlayer = obj.MessageObject as HypergramPlayer;
                            hypergramGameService.Pick(SelectedPlayer);
                            StateHasChanged();
                        }
                        break;
                    case MessageModel.MessageType.Change:
                        SelectedPlayer = obj.MessageObject as HypergramPlayer;
                        hypergramGameService.Change();
                        StateHasChanged();
                        break;
                    case MessageModel.MessageType.Pass:
                        {
                            SelectedPlayer = obj.MessageObject as HypergramPlayer;
                            var round = hypergramGameService.Pass();
                            messageService.SendMessage(new MessageModel()
                            {
                                Type = MessageModel.MessageType.RoundIsPlayed,
                                MessageObject = round
                            });
                            StateHasChanged();
                        }
                        break;
                    case MessageModel.MessageType.RoundIsPlayed:
                        {
                            HypergramGameRound round = obj.MessageObject as HypergramGameRound;
                            LastRound = hypergramGameService.SetPlayedRound(SelectedPlayer, round);
                            messageService.SendMessage(new MessageModel()
                            {
                                Type = MessageModel.MessageType.SetNextPlayer
                            });
                            StateHasChanged();
                        }
                        break;
                    case MessageModel.MessageType.SetNextPlayer:
                        {
                            hypergramGameService.SetNextPlayer();
                            StateHasChanged();
                        }
                        break;

                }
            }));
        }

        public async Task StartGame()
        {
            await InvokeAsync(() =>
            {
                hypergramGameService.StartGame();
                messageService.SendMessage(new Models.MessageModel()
                {
                    Type = Models.MessageModel.MessageType.GameStarted
                });
            });
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
