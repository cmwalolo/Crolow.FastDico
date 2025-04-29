using Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.Utils;
using System.Runtime.CompilerServices;
using static Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators.Evaluator;

namespace Crolow.FastDico.ScrabbleApi.Components.Rounds
{

    public class XRoundValidator : BaseRoundValidator
    {

        int maxLettersInRack = 100;
        int currentIteration = 0;

        int[] maxIteration = new int[] { 50, 30, 30 };
        int[] breakPoints = new int[] { 15, 13, 10 };

        private int boostNumberOfSolutions = 3000;
        private int boostMatchItems = 100;

        private Evaluator evaluator;
        private RatingRound bestRate;
        private PlayedRounds bestRounds;

        public XRoundValidator(CurrentGame currentGame) : base(currentGame)
        {
            evaluator = new Evaluator(currentGame);
        }

        public override void Initialize()
        {
            currentIteration = 0;
            int[] maxIteration = new int[] { 100, 50, 50 };
            int[] breakPoints = new int[] { 13, 11, 8 };


            evaluator.Initialize();
            bestRate = null;
            bestRounds = null;
        }

        public override List<Tile> InitializeLetters()
        {
            if (!evaluator.IsBoosted())
            {
                return currentGame.LetterBag.DrawLetters(currentGame.Rack);
            }
            else
            {
                return currentGame.LetterBag.DrawLetters(currentGame.Rack, maxLettersInRack);
            }
        }


        public override SolverFilters InitializeFilters()
        {
            SolverFilters f = new SolverFilters();
            f.PickallResults = evaluator.IsBoosted();
            return f;
        }

        public override PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, PlayerRack originalRack)
        {
            if (currentGame.Round == 0)
            {
                currentGame.LetterBag.ReturnLetters(currentGame.Rack, letters);
                currentGame.LetterBag.Recreate(currentGame.Rack, originalRack);
                return rounds;
            }

            if (evaluator.IsBoosted())
            {
                currentGame.LetterBag.ReturnLetters(currentGame.Rack, letters);
                currentGame.LetterBag.Recreate(currentGame.Rack, originalRack);
                return rounds;
            }
            else
            {
                currentGame.LetterBag.ReturnLetters(currentGame.Rack, letters);
                currentGame.LetterBag.Recreate(currentGame.Rack, originalRack);

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
        }

        public override PlayedRound FinalizeRound(PlayedRounds playedRounds)
        {
            if (!evaluator.IsBoosted())
            {
                return base.FinalizeRound(playedRounds);
            }

#if DEBUG
            Console.WriteLine("--- BOOSTED --- ");
#endif
            if (playedRounds.Tops.Count == 0)
            {
                return null;
            }


            var solutions = new List<PlayedRound>();

            if (currentGame.GameConfig.JokerMode)
            {
                solutions = playedRounds.AllRounds.Where(p => p.Tiles.Count > 7 && p.Tiles.Any(p => p.IsJoker)).OrderByDescending(p => p.Points).ToList();
            }

            if (!solutions.Any())
            {
                solutions = playedRounds.AllRounds.Where(p => p.Tiles.Count > 7).OrderByDescending(p => p.Points).ToList();
            }

            if (!solutions.Any())
            {
                solutions = playedRounds.AllRounds.OrderByDescending(p => p.Points).ToList();
            }

            var selectionLong = new Dictionary<RatingRound, PlayedRound>();
            var selectionShort = new Dictionary<RatingRound, PlayedRound>();
            var scrabbles = new Dictionary<RatingRound, PlayedRound>();
            var counter = 0;
            foreach (var solution in solutions)
            {
                var rate = evaluator.Evaluate(playedRounds, solution);
                if (solution.Tiles.Count(p => p.Parent.Status == -1) >= 5 && (rate.scoreappui > 2 || rate.scorecollage > 5))
                {
                    selectionLong.Add(rate, solution);
                }
                else
                {
                    if (solution.Bonus > 0)
                    {
                        scrabbles.Add(rate, solution);
                    }

                    if (rate.scoreappui > 2 || rate.scorecollage > 5)
                    {
                        selectionShort.Add(rate, solution);
                    }
                }

                counter++;

                if (selectionLong.Count > boostMatchItems && counter < boostNumberOfSolutions)
                {
                    break;
                }
            }

#if DEBUG
            Console.WriteLine($"BOOSTING {solutions.Count} - matches : {selection.Count} ");
#endif

            var keys = selectionLong.OrderByDescending(p => (p.Key.scoreappui * 2) + p.Key.scorecollage).FirstOrDefault();
            if (keys.Value == null)
            {
                keys = selectionShort.OrderByDescending(p => (p.Key.scoreappui * 2) + p.Key.scorecollage).FirstOrDefault();

                if (keys.Value == null && scrabbles.Count > 0)
                {
                    int c= Random.Shared.Next(scrabbles.Count);
                    keys = scrabbles.ElementAt(c);
                }
            }

            var selectedRound = keys.Value ?? solutions.FirstOrDefault();

            // We remove letters played from the rack
            selectedRound.Rack = new PlayerRack(selectedRound.Tiles.Where(p => p.Parent.Status == -1).ToList());
            currentGame.Rack = new PlayerRack();
            //currentGame.LetterBag.ReturnLetters(currentGame.Rack, playedRounds.PlayerRack.Tiles);

            foreach (var letter in selectedRound.Rack.Tiles)
            {
                currentGame.LetterBag.RemoveTile(letter);
            }
            selectedRound.FinalizeRound();
            return selectedRound;

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
