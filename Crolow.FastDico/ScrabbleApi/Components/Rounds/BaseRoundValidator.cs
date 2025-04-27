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

        public void Initialize()
        {

        }

        public PlayedRounds ValidateRound(PlayedRounds rounds)
        {
            return rounds;
        }
    }
}
