using Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using static Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators.Evaluator;

namespace Crolow.FastDico.ScrabbleApi.Components.Rounds
{
    public class XRoundValidator : BaseRoundValidator
    {
        int currentIteration = 0;
        int[] maxIteration = new int[] { 50, 30, 30 };
        int[] breakPoints = new int[] { 15, 13, 10 };

        private Evaluator evaluator;
        private RatingRound bestRate;
        private PlayedRounds bestRounds;

        public XRoundValidator(CurrentGame currentGame) : base(currentGame)
        {
            evaluator = new Evaluator(currentGame);
        }

        public void Initialize()
        {
            currentIteration = 0;
            int[] maxIteration = new int[] { 100, 50, 50 };
            int[] breakPoints = new int[] { 13, 11, 8 };


            evaluator.Initialize();
            bestRate = null;
            bestRounds = null;
        }

        public PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters)
        {

            if (currentGame.Round == 0)
            {
                return rounds;
            }

            var rate = evaluator.Evaluate(rounds);

            if (rate.scoreAll > breakPoints[currentIteration])
            {
                DebugRatingRound(rate);
                return rounds;
            }
            else
            {
                maxIteration[currentIteration]--;

                if (maxIteration[currentIteration] == 0)
                {
                    currentIteration++;
                    if (currentIteration == maxIteration.Length)
                    {
                        DebugRatingRound(bestRate);
                        return bestRounds;
                    }
                    else
                    {
                        if (bestRate.scoreAll > breakPoints[currentIteration])
                        {
                            DebugRatingRound(bestRate);
                            return bestRounds;
                        }
                    }
                }

                if (bestRate == null || rate.scoreAll > bestRate.scoreAll)
                {
                    bestRate = rate;
                    bestRounds = rounds;
                }

                return null;
            }


            return rounds;
        }

        public void DebugRatingRound(RatingRound round)
        {
#if DEBUG
            Console.WriteLine("Score rack : " + round.scorerack);
            Console.WriteLine("Score collage  : " + round.scorecollage);
            Console.WriteLine("Score collagemots : " + round.scorecollagemots);
            Console.WriteLine("Score mot : " + round.scoremot);
            Console.WriteLine("Score raccords : " + round.scoreraccords);
            Console.WriteLine("Score scrabble : " + round.scorescrabble);
            Console.WriteLine("Score soustop : " + round.scoresoustop);
            Console.WriteLine("Score appui : " + round.scoreappui);
            Console.WriteLine("Overall : " + round.scoreAll);
            Console.WriteLine("---------------------------------");
#endif
        }
    }
}
