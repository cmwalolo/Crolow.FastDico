using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;

namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{
    public interface IBaseRoundValidator
    {
        bool CanRejectBagByDefault(LetterBag bag, PlayerRack rack);
        PlayableSolution FinalizeRound(PlayedRounds playedRounds);
        void Initialize();
        SolverFilters InitializeFilters();
        List<Tile> InitializeLetters();
        void InitializeRound();
        PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, IBoardSolver solver);
    }
}