namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{
    public interface IScrabbleAI
    {
        void EndGame();
        void NextRound(bool firstMove);
        void PrintGrid();
        void StartGame();
    }
}