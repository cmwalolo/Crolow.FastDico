namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{

    public interface IScrabbleAI
    {
        public delegate void RoundIsReadyEvent();

        event RoundIsReadyEvent RoundIsReady;

        void EndGame();
        void NextRound(bool firstMove);
        void StartGame();
    }
}