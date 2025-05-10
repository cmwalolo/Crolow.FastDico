using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.ScrabbleApi.Extensions;
using Crolow.FastDico.ScrabbleApi.Utils;
using Newtonsoft.Json.Bson;
using System.Diagnostics.Metrics;
using System.Text;
using static Crolow.FastDico.Common.Interfaces.ScrabbleApi.IScrabbleAI;

namespace Crolow.FastDico.ScrabbleApi;

public class ScrabbleAI : IScrabbleAI
{
    //public delegate void RoundIsReadyEvent();

    public event RoundIsReadyEvent? RoundIsReady;

    private CurrentGame CurrentGame;

    public ScrabbleAI(CurrentGame currentGame)
    {
        this.CurrentGame = currentGame;
    }

    public async void StartGame()
    {
        NextRound(true);
    }
    public async void NextRound(bool firstMove)
    {
        using (StopWatcher stopwatch = new StopWatcher("New round"))
        {
            CurrentGame.ControllersSetup.BoardSolver.Initialize();
            CurrentGame.ControllersSetup.Validator.Initialize();

            PlayedRounds playedRounds = null;
            var letters = new List<Tile>();

            // We create a copy of the rack and the back to
            // Start freshly each iteration
            var originalRack = new PlayerRack(CurrentGame.GameObjects.Rack);
            var originalBag = new LetterBag(CurrentGame.GameObjects.LetterBag);

            while (true)
            {
                CurrentGame.ControllersSetup.Validator.InitializeRound();
                letters = CurrentGame.ControllersSetup.Validator.InitializeLetters();
                // End Test
                if (letters == null)
                {
                    EndGame();
                    return;
                }

                var filters = CurrentGame.ControllersSetup.Validator.InitializeFilters();
                playedRounds = CurrentGame.ControllersSetup.BoardSolver.Solve(letters, filters);

                if (playedRounds.Tops.Any())
                {
                    var round = CurrentGame.ControllersSetup.Validator.ValidateRound(playedRounds, letters, CurrentGame.ControllersSetup.BoardSolver);
                    if (round != null)
                    {
                        playedRounds = round;
                        break;
                    }
                }
                else
                {
                    EndGame();
                    return;
                }

                CurrentGame.GameObjects.Rack = originalRack;
                CurrentGame.GameObjects.LetterBag = originalBag;
            }

            PlayableSolution selectedRound = CurrentGame.ControllersSetup.Validator.FinalizeRound(playedRounds);
            if (selectedRound == null)
            {
                EndGame();
                return;
            }

            CurrentGame.GameObjects.SelectedRound = selectedRound;
            CurrentGame.GameObjects.GameStatus = GameStatus.WaitingForNextRound;
        }

        if (RoundIsReady != null)
        {
            RoundIsReady.Invoke();
        }
        else
        {
            SetRound();
        }
    }

    public async void SetRound()
    {
        var selectedRound = CurrentGame.GameObjects.SelectedRound;

        CurrentGame.GameObjects.Board.SetRound(selectedRound);
        CurrentGame.GameObjects.RoundsPlayed.Add(selectedRound);

        CurrentGame.GameObjects.Round++;
#if DEBUG
        CurrentGame.GameObjects.LetterBag.DebugBag(CurrentGame.GameObjects.Rack);
#endif

        NextRound(false);
    }

    public void EndGame()
    {
        CurrentGame.GameObjects.GameStatus = GameStatus.GameEnded;
    }

}