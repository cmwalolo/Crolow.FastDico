using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi.Components.Rounds
{
    public class BaseRoundValidator
    {
        public CurrentGame currentGame;
        public BaseRoundValidator(CurrentGame currentGame)
        {
            this.currentGame = currentGame;
        }

        public virtual void Initialize()
        {

        }

        public virtual List<Tile> InitializeLetters()
        {
            return currentGame.LetterBag.DrawLetters(currentGame.Rack);
        }
        public virtual PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, BoardSolver solver)
        {
            //currentGame.LetterBag.ReturnLetters(currentGame.Rack);
            //currentGame.LetterBag.Recreate(currentGame.Rack, originalRack);

            return rounds;
        }

        public virtual SolverFilters InitializeFilters()
        {
            return new SolverFilters();
        }


        public virtual PlayedRound FinalizeRound(PlayedRounds playedRounds)
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
