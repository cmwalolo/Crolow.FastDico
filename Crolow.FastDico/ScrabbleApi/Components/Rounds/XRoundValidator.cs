using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators;
using Crolow.FastDico.ScrabbleApi.Extensions;
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
        int[] breakPoints = new int[] { 20, 15, 10 };

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
            maxIteration = new int[] { 100, 50, 50 };
            breakPoints = new int[] { 13, 11, 8 };

            evaluator.Initialize();
            bestRate = null;
            bestRounds = null;
        }

        public virtual void InitializeRound()
        {
        }


        public override List<Tile> InitializeLetters()
        {
            if (!evaluator.IsBoosted())
            {
                return currentGame.GameObjects.LetterBag.DrawLetters(currentGame.GameObjects.Rack);
            }
            else
            {
                return currentGame.GameObjects.LetterBag.DrawLetters(currentGame.GameObjects.Rack, maxLettersInRack);
            }
        }


        public override SolverFilters InitializeFilters()
        {
            SolverFilters f = new SolverFilters();
            f.PickallResults = evaluator.IsBoosted();
            return f;
        }

        public override PlayedRounds ValidateRound(PlayedRounds rounds, List<Tile> letters, IBoardSolver solver)
        {
            if (solver is null)
            {
                throw new ArgumentNullException(nameof(solver));
            }

            if (currentGame.GameObjects.Round == 0)
            {
                return rounds;
            }

            if (evaluator.IsBoosted())
            {
                rounds = ValidateBoosted(rounds, solver);
                if (rounds == null)
                {
                    evaluator.BoostedOff();
                }

                return rounds;
            }
            else
            {
                var rate = evaluator.Evaluate(rounds);

                if (rate.scoreAll > breakPoints[currentIteration])
                {
                    DebugRatingRound(rate);
                    return rounds;
                }
                else
                {
                    maxIteration[currentIteration]--;

                    if (maxIteration[currentIteration] <= 0)
                    {
                        currentIteration++;
                        if (currentIteration == maxIteration.Length)
                        {
                            DebugRatingRound(bestRate);
                            return bestRounds;
                        }
                        else
                        {
                            if (bestRate == null)
                            {
                                Console.WriteLine("WTF");
                            }
                            if (bestRate != null && (bestRate.scoreAll > breakPoints[currentIteration]))
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

        private PlayedRounds ValidateBoosted(PlayedRounds playedRounds, IBoardSolver solver)
        {
#if DEBUG
            Console.WriteLine("--- BOOSTED --- ");
#endif
            if (playedRounds.Tops.Count == 0)
            {
                return null;
            }


            var solutions = new List<PlayableSolution>();

            if (currentGame.GameObjects.GameConfig.JokerMode)
            {
                solutions = playedRounds.AllRounds
                    .Distinct()
                    .Where(p => p.Tiles.Count > 4 && p.Tiles.Any(p => p.IsJoker)).ToList();
            }

            if (!solutions.Any())
            {
                solutions = playedRounds.AllRounds.Distinct().Where(p => p.Tiles.Count > 4).OrderByDescending(p => p.Points).ToList();
            }

            solutions = solutions.OrderByDescending(p => p.Points).ToList();

            var selection = new Dictionary<RatingRound, PlayableSolution>();
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

            var keys = selection.OrderByDescending(p => p.Key.scoreAll /* (p.Key.scoreappui * 2) + p.Key.scorecollage*/);

            currentGame.GameObjects.LetterBag.ReturnLetters(currentGame.GameObjects.Rack);

            // For each solution we need to check that the selected solution
            // with a rack is the top score. We only do one trial. 
            // If solution is not top score we pass to next . 
            // If no solution is ok we return a null and 
            // process will fallback to unboosted validation

            bool found = false;
            foreach (var value in keys)
            {
                for (int x = 0; x < 10; x++)
                {
                    var selectedSolution = value.Value;
                    var rack = new PlayerRack(value.Value.Tiles.Where(p => p.Parent.Status == -1).ToList());

                    // No need to try out different racks as it is full
                    if (rack.Tiles.Count == currentGame.GameObjects.GameConfig.PlayableLetters)
                    {
                        x = 10;
                    }

                    currentGame.GameObjects.LetterBag.ForceDrawLetters(rack.Tiles);
                    var letters = currentGame.GameObjects.LetterBag.DrawLetters(rack);
                    var round = solver.Solve(letters);
                    currentGame.GameObjects.LetterBag.ReturnLetters(rack);
                    selectedSolution.Rack = new PlayerRack(letters);
                    var checkSolution = round.Tops.FirstOrDefault();

                    if (selectedSolution.ToString() == checkSolution.ToString()
                        && checkSolution.Position.Equals(selectedSolution.Position))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
#if DEBUG
                    Console.WriteLine($"BOOSTED");
                    DebugRatingRound(value.Key);
#endif

                    playedRounds.AllRounds.Clear();
                    playedRounds.SubTops.Clear();
                    playedRounds.Tops.Clear();
                    playedRounds.Tops.Add(value.Value);
                    return playedRounds;
                }
            }

            return null;

        }

        public override PlayableSolution FinalizeRound(PlayedRounds playedRounds)
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
            currentGame.GameObjects.Rack = new PlayerRack();
            //currentGame.LetterBag.ReturnLetters(currentGame.Rack, playedRounds.PlayerRack.Tiles);

            foreach (var letter in selectedRound.Rack.Tiles)
            {
                currentGame.GameObjects.LetterBag.RemoveTile(letter);
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
