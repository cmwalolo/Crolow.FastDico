using Crolow.FastDico.ScrabbleApi.GameObjects;

namespace Crolow.FastDico.ScrabbleApi.Components.Rounds
{
    public class XRoundValidator : BaseRoundValidator
    {
        int maxIteration = 50;
        private List<PlayedRounds> rounds = new List<PlayedRounds>();

        public XRoundValidator(CurrentGame currentGame) : base(currentGame)
        {
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
