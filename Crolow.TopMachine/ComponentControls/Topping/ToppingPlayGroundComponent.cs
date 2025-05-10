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

        protected async override void OnInitialized()
        {
            currentGame = ApplicationContext.CurrentGame;
            currentGame.ControllersSetup.ScrabbleEngine.RoundIsReady += ScrabbleEngine_RoundIsReady;
        }

        private async void ScrabbleEngine_RoundIsReady()
        {
            await SendMessage("InitializeRound");
        }

        protected async void StartGame()
        {
            currentGame.ControllersSetup.ScrabbleEngine.StartGame();
            isStartGameButtonDisabled = true;
            //startGameButton.Disabled = false;
            StateHasChanged();
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
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ApplicationContext.CurrentGame.GameObjects);
            try
            {
                var result = await jsRuntime.InvokeAsync<object>("messageService.sendMessage", message, jsonString);
            }
            catch (Exception ex) { }
        }

        public void Dispose()
        {

        }
    }
}
