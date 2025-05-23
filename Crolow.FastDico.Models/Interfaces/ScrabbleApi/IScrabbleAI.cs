using Crolow.FastDico.Common.Models.ScrabbleApi.Game;

namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{

    public interface IScrabbleAI
    {
        public delegate void RoundIsReadyEvent();
        public delegate void RoundSelectedEvent(PlayableSolution solution, PlayerRack rack);

        event RoundIsReadyEvent RoundIsReady;
        event RoundSelectedEvent RoundSelected;

        void EndGame();
        Task<bool> NextRound(bool firstMove = false);
        Task StartGame();
        void SetRound();
        Task<bool> ValidateRound(PlayableSolution solution);


    }
}