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
        void NextRound(bool firstMove = false);
        void StartGame();
        void SetRound();
        void ValidateRound(PlayableSolution solution);

    }
}