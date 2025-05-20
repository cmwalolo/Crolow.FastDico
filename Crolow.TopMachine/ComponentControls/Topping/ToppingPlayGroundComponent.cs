using Crolow.FastDico.Common.Models.Common;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;

namespace Crolow.TopMachine.ComponentControls.Topping
{
    public class ToppingPlayGroundComponent : ComponentBase, IDisposable
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        public CurrentGame currentGame;

        [Inject]
        IJSRuntime jsRuntime { get; set; }

        public RadzenMenuItem startGameButton;

        private DotNetObjectReference<ToppingPlayGroundComponent>? playGroundComponentRef;


        protected async override void OnInitialized()
        {
            playGroundComponentRef = DotNetObjectReference.Create(this);
            await jsRuntime.InvokeVoidAsync("setPlaygroundComponent", playGroundComponentRef);
            currentGame = ApplicationContext.CurrentGame;
            currentGame.ControllersSetup.ScrabbleEngine.RoundIsReady += ScrabbleEngine_RoundIsReady;
            currentGame.ControllersSetup.ScrabbleEngine.RoundSelected += ScrabbleEngine_RoundSelected;
        }

        private async void ScrabbleEngine_RoundSelected(PlayableSolution solution, PlayerRack rack)
        {
            await SendMessageWithObject("SetRound", new object[] { solution, rack });
        }

        private async void ScrabbleEngine_RoundIsReady()
        {
            await SendMessage("InitializeRound");
        }

        protected async void StartGame()
        {
            await InvokeAsync(async () =>
            {
                currentGame.ControllersSetup.ScrabbleEngine.StartGame();
                isStartGameButtonDisabled = true;
                StateHasChanged();
            });
        }

        public class InternalPlayableSolution
        {
            public List<Square> Squares { get; set; }
            public Position Position { get; set; }
            public float PlayedTime { get; set; }
            public PlayerRack Rack { get; set; }
            public bool FinalRound { get; set; }
        }

        [JSInvokable]
        public async void Validate(InternalPlayableSolution solution)
        {
            await InvokeAsync(async () =>
            {
                PlayableSolution s = new PlayableSolution
                {
                    Tiles = solution.Squares.Select(p => p.CurrentLetter).ToList(),
                    Position = solution.Position,
                    PlayedTime = solution.PlayedTime,
                    Rack = solution.Rack,
                };


                //currentGame.ControllersSetup.ScrabbleEngine.V
            });
        }


        protected async void NextRound()
        {
            await InvokeAsync(async () =>
            {
                currentGame.ControllersSetup.ScrabbleEngine.SetRound();
                currentGame.ControllersSetup.ScrabbleEngine.NextRound();
                isStartGameButtonDisabled = true;
                StateHasChanged();
            });
        }

        public bool isStartGameButtonDisabled = true;
        protected async override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await SendMessage("InitializeGame");
                if (currentGame.GameObjects.GameStatus == GameStatus.WaitingToStart)
                {
                    isStartGameButtonDisabled = false;
                    StateHasChanged();
                }
            }

            base.OnAfterRender(firstRender);
        }

        private async Task SendMessage(string message)
        {
            PrintGame.PrintGrid(currentGame);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ApplicationContext.CurrentGame.GameObjects);
            try
            {
                var result = await jsRuntime.InvokeAsync<object>("messageService.sendMessage", message, jsonString);
            }
            catch (Exception ex) { }
        }

        private async Task SendMessageWithObject(string message, object o)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(o);

            try
            {
                var result = await jsRuntime.InvokeAsync<object>("messageService.sendMessage", message, jsonString);
            }
            catch (Exception ex) { }
        }

        public void Dispose()
        {
            playGroundComponentRef?.Dispose();
        }
    }
}
