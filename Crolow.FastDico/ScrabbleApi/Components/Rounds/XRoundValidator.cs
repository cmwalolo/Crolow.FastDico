using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.Utils;
using System.Reflection.Metadata.Ecma335;
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

        public override PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, PlayerRack originalRack, BoardSolver solver)
        {
            if (solver is null)
            {
                throw new ArgumentNullException(nameof(solver));
            }

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

                rounds = ValidateBoosted(rounds, solver);
                if (rounds == null)
                {
                    evaluator.BoostedOff();
                }

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

        private PlayedRounds ValidateBoosted(PlayedRounds playedRounds, BoardSolver solver)
        {
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

            var selection = new Dictionary<RatingRound, PlayedRound>();
            var counter = 0;
            foreach (var solution in solutions)
            {
                var rate = evaluator.Evaluate(playedRounds, solution);
                if (solution.Tiles.Count(p => p.Parent.Status == -1) > 6 && rate.scoreAll > 15 && (rate.scoreappui > 2 || rate.scorecollage > 5))
                {
                    selection.Add(rate, solution);
                }

                counter++;

                if (selection.Count > boostMatchItems && counter < boostNumberOfSolutions)
                {
                    break;
                }
            }

#if DEBUG
            Console.WriteLine($"BOOSTING {solutions.Count} - matches : {selection.Count} ");
#endif 

            if (!selection.Any())
            {
                return null;
            }

            var keys = selection.OrderByDescending(p => (p.Key.scoreappui * 2) + p.Key.scorecollage);

            foreach (var value in keys)
            {
                var selectedSolution = value.Value;

                var rack = new PlayerRack(value.Value.Tiles.Where(p => p.Parent.Status == -1).ToList());
                currentGame.LetterBag.ForceDrawLetters(rack.Tiles);
                var letters = currentGame.LetterBag.DrawLetters(rack);
                var round = solver.Solve(letters);
                currentGame.LetterBag.ReturnLetters(letters);

                var checkSolution = round.Tops.FirstOrDefault();


                if (selectedSolution.ToString() != checkSolution.ToString()
                    || !checkSolution.Position.Equals(selectedSolution.Position))
                {
                    continue;
                }

#if DEBUG
                DebugRatingRound(value.Key);
#endif

                playedRounds.AllRounds.Clear();
                playedRounds.SubTops.Clear();
                playedRounds.Tops.Clear();
                playedRounds.Tops.Add(value.Value);
                return playedRounds;
            }

            return null;

        }

        public override PlayedRound FinalizeRound(PlayedRounds playedRounds)
        {
            if (!evaluator.IsBoosted())
            {
                return base.FinalizeRound(playedRounds);
            }

            if (playedRounds.Tops.Count == 0)
            {
                return null;
            }
            var selectedRound = playedRounds.Tops.FirstOrDefault();

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
            if (round != null)
            {
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
            }
#endif
        }
    }
}
