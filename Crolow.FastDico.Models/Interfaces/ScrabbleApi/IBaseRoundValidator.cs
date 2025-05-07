using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;

namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{
    public interface IBaseRoundValidator
    {
        PlayableSolution FinalizeRound(PlayedRounds playedRounds);
        void Initialize();
        SolverFilters InitializeFilters();
        List<Tile> InitializeLetters();
        PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, IBoardSolver solver);
    }
}