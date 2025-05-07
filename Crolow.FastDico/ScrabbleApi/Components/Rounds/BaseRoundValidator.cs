using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Extensions;

namespace Crolow.FastDico.ScrabbleApi.Components.Rounds
{
    public class BaseRoundValidator : IBaseRoundValidator
    {
        public CurrentGame currentGame;
        public BaseRoundValidator(CurrentGame currentGame)
        {
            this.currentGame = currentGame;
        }

        public virtual void Initialize()
        {

        }
        public virtual void InitializeRound()
        {
        }

        public virtual List<Tile> InitializeLetters()
        {
            return currentGame.LetterBag.DrawLetters(currentGame.Rack);
        }
        public virtual PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, IBoardSolver solver)
        {
            return rounds;
        }

        public virtual SolverFilters InitializeFilters()
        {
            return new SolverFilters();
        }
        public virtual PlayableSolution FinalizeRound(PlayedRounds playedRounds)
        {
            if (playedRounds.Tops.Count == 0)
            {
                return null;
            }

            var rnd = Random.Shared.Next(playedRounds.Tops.Count);
            var selectedRound = playedRounds.Tops[rnd];

            // We remove letters played from the rack
            currentGame.LetterBag.ReturnLetters(currentGame.Rack);
            selectedRound.Rack = new PlayerRack(playedRounds.PlayerRack);
            currentGame.Rack = playedRounds.PlayerRack;
            currentGame.LetterBag.ForceDrawLetters(currentGame.Rack.Tiles);

            foreach (var letter in selectedRound.Tiles)
            {
                if (letter.Parent.Status != 1)
                {
                    currentGame.Rack.RemoveTile(letter);
                }
            }
            selectedRound.FinalizeRound();
            return selectedRound;
        }

    }
}
