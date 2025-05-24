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
            var reject = this.CanRejectBagByDefault(currentGame.GameObjects.LetterBag, currentGame.GameObjects.Rack);
            return currentGame.GameObjects.LetterBag.DrawLetters(currentGame.GameObjects.Rack, reject: reject);
        }
        public virtual PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, IBoardSolver solver)
        {
            return rounds;
        }

        public virtual bool CanRejectBagByDefault(LetterBag bag, PlayerRack rack)
        {
            return false;
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
            currentGame.GameObjects.LetterBag.ReturnLetters(currentGame.GameObjects.Rack);
            selectedRound.Rack = new PlayerRack(playedRounds.PlayerRack);
            currentGame.GameObjects.Rack = playedRounds.PlayerRack;
            currentGame.GameObjects.LetterBag.ForceDrawLetters(currentGame.GameObjects.Rack.Tiles);

            foreach (var letter in selectedRound.Tiles)
            {
                if (letter.Parent.Status != 1)
                {
                    currentGame.GameObjects.Rack.RemoveTile(letter);
                }
            }
            selectedRound.FinalizeRound();
            return selectedRound;
        }

    }
}
