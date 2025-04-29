using Crolow.FastDico.GadDag;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using System.Collections;



namespace Crolow.FastDico.ScrabbleApi.Components.Rounds.Evaluators
{
    public class Evaluator
    {
        private CurrentGame currentGame;
        public class RoundsComparer : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(object x, object y)
            {
                return ((RatingRound)x).scoreAll < ((RatingRound)y).scoreAll ? 0 : 1;
            }

        }

        bool doCollages = false;
        bool doScrabble = false;
        bool doAppuis = false;
        bool doRaccords = false;
        bool doRack = false;
        bool doBoost = false;


        public class RatingRound
        {
            public bool NoResults = false;
            public bool Valid = true;
            public bool rejet = false;
            public int nbSolutions = 0;
            public int nbTirages = 0;
            public float scoreAll = 0;
            public float scorerack = 1;
            public float scoresoustop = 0;
            public float scorescrabble = 0;
            public float scoreraccords = 0;
            public float scorecollage = 0;
            public float scorecollagemots = 0;
            public float scoremot = 0;
            public float scoreappui = 0;
        }

        public int[] Tiles_points = new int[]
        {
			    /* x A B C D  E F G H I J  K L M N O P Q R S T U V  W  X  Y  Z ? */
			    0,1,3,3,2, 1,4,2,4,1,8,10,1,2,1,1,3,8,1,1,1,1,4,10,10,10,10,0
        };


        public const int RescanResultNumber = 4;
        public const float sousTopRatioDivScrabble = 50;
        public const float sousTopRatioDiv = 20;
        public const float scoreMotRatioMul = 1;

        public const float RackRatioMult = 1;           // Multiplier
        public const float ScrabbleRatioDiv = 25;
        public const float RaccordsRatioMul = 1;
        public const float CollageRatioDiv = 1;
        public const float CollageMotRatioDiv = 4;
        public const float AppuiRatioDiv = 4;

        public int ScrabbleFrequence = 60;
        public int AppuisFrequence = 80;
        public int CollagesFrequence = 80;
        public int RaccordsFrequence = 80;
        public int RackFrequence = 40;
        public int BoostFrequence = 20;

        private int maxTurn = 30;

        public Evaluator(CurrentGame currentGame)

        {
            this.currentGame = currentGame;
            Random rnd = new Random();

            ScrabbleFrequence = 40;  //cfg.Config.intScrabbleFrequence;
            CollagesFrequence = 80;  //cfg.Config.intCollagesFrequence;
            AppuisFrequence = 80;  //cfg.Config.intAppuisFrequence;
            RaccordsFrequence = 80; //  cfg.Config.intRaccordsFrequence;
            RackFrequence = 40; //  cfg.Config.intRackFrequence; 
            BoostFrequence = 30;
        }

        public bool IsBoosted()
        {
            return doBoost;
        }

        public void Initialize()
        {
            int c = Random.Shared.Next(100);

            doCollages = doScrabble = doAppuis = false;
            doRaccords = doRack = doBoost = false;

            if (c < CollagesFrequence)
            {
                doCollages = true;
            }

            c = Random.Shared.Next(100);

            if (c < ScrabbleFrequence)
            {
                doScrabble = true;
            }

            c = Random.Shared.Next(100);
            if (c < AppuisFrequence)
            {
                doAppuis = true;
            }

            c = Random.Shared.Next(100);
            if (c < RaccordsFrequence)
            {
                doRaccords = true;
            }

            c = Random.Shared.Next(100);
            if (c < RackFrequence)
            {
                doRack = true;
            }

            c = Random.Shared.Next(100);
            if (c < BoostFrequence && currentGame.Round > 3)
            {
                doBoost = true;
            }
        }

        public RatingRound Evaluate(PlayedRounds round, PlayedRound selectedRound = null)
        {

            float maxScore = 0;
            int maxItem = 0;
            int item = 0;

            RatingRound rate = new();
            selectedRound = selectedRound ?? round.Tops.FirstOrDefault();
            if (selectedRound != null)
            {
                string word = selectedRound.GetWord();

                EvaluateNumberOfSolutions(rate, round);

                if (doCollages || doBoost)
                {
                    // EvaluateCollagesMots(rate, selectedRound, word);
                    EvaluateCollages(rate, selectedRound, word);
                }

                if (doAppuis || doBoost)
                {
                    EvaluateScoreAppui(rate, selectedRound, word);
                }

                EvaluateScrabble(rate, selectedRound, word);

                if (!doBoost)
                {
                    if (doScrabble)
                    {
                        EvaluateScrabbleSousTop(rate, selectedRound, word, round.SubTops.FirstOrDefault());
                    }
                    else
                    {
                        EvaluateSousTop(rate, selectedRound, word, round.SubTops.FirstOrDefault());
                    }
                }

                EvaluateScoreMot(rate, selectedRound, word);

                if (!doBoost)
                {
                    if (doRaccords)
                    {
                        EvaluateRaccords(rate, selectedRound, word);
                    }


                    if (doScrabble && selectedRound.Bonus == 0)
                    {
                        rate.scoreAll = rate.scoreAll - 3;
                    }
                    else
                    {
                        if (selectedRound.Bonus > 0)
                        {
                            rate.scoreAll = rate.scoreAll - 3;
                        }
                    }
                }
            }

            rate.scoreAll = rate.scoreAll / rate.nbSolutions;
            return rate;

        }

        /// <summary>
        /// Evaluate number of solutions possible 
        /// This will be a ratio that will decrease the global rating 
        /// If multiple solutions 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="round"></param>
        private void EvaluateNumberOfSolutions(RatingRound rate, PlayedRounds round)
        {
            rate.nbSolutions = round.Tops.Count;
        }

        private void EvaluateScoreMot(RatingRound rate, PlayedRound round, string word)
        {
            //if (round.Bonus == 0)
            {
                if (word.Length > 9)
                {
                    rate.scoremot = 3;
                }

                if (word.Length > 7)
                {
                    rate.scoremot = 2.5f;
                }

                if (word.Length <= 7)
                {
                    rate.scoremot = 2;
                }

                if (word.Length < 4)
                {
                    rate.scoremot = -15;
                }

                rate.scoreAll += rate.scoremot * scoreMotRatioMul;
            }
        }

        private void EvaluateScoreAppui(RatingRound rate, PlayedRound round, string word)
        {
            if (word.Length < 5)
            {
                return;
            }

            int c = 0;
            for (int x = 0; x < word.Length; x++)
            {
                if (round.Tiles[x].Parent.Status == 1)
                {
                    c++;
                }
            }

            //c = word.Length - c;

            if (c > 1 && c < word.Length - 1)
            {
                rate.scoreappui = c;
                rate.scoreAll += rate.scoreappui * AppuiRatioDiv;
            }
        }

        private void EvaluateCollages(RatingRound rate, PlayedRound round, string word)
        {
            int length = word.Length;

            int count = 0; // gc.GameBoard.evalRaccords(tround);
            int letters = 0;
            foreach (var l in round.Tiles)
            {
                if (l.Parent.Status == -1)
                {
                    int c = l.Parent.GetPivotLetters(round.Position.Direction);
                    if (c > 0)
                    {
                        count += System.Math.Min(3, c);
                        letters++;
                    }
                }
            }

            if (count > 0)
            {
                rate.scorecollage = count * letters;
                rate.scoreAll += ((float)rate.scorecollage * CollageRatioDiv);
            }
        }

        /* private void EvaluateCollagesMots(RatingRound rate, PlayedRound round, string word)
         {
             int length = word.Length;

             int count = 0; // ((CBoard)gc.GameBoard).evalRaccordsMots(tround);

             if (count > 3)
             {
                 rate.scorecollagemots = count;
                 rate.scoreAll += count / (CollageMotRatioDiv * rate.nbSolutions);
             }

         }*/

        private void EvaluateRaccords(RatingRound rate, PlayedRound round, string word)
        {
            if (word.Length > 4)
            {
                int count = CompteRaccord(word);
                rate.scoreraccords = count;
                rate.scoreAll += count * RaccordsRatioMul;
            }
        }

        private void EvaluateScrabble(RatingRound rate, PlayedRound round, string word)
        {
            if (round.Bonus > 0)
            {
                rate.scorescrabble = (float)round.Bonus / ScrabbleRatioDiv;
                rate.scoreAll += (float)rate.scorescrabble;
            }
        }

        private void EvaluateScrabbleSousTop(RatingRound rate, PlayedRound round, string word, PlayedRound subTop)
        {
            if (round.Bonus > 0)
            {
                float diff = 100 - subTop.Points / (float)round.Points * 100;
                rate.scoresoustop = diff / sousTopRatioDivScrabble;
                rate.scoreAll += rate.scoresoustop;
            }
        }

        private void EvaluateSousTop(RatingRound rate, PlayedRound round, string word, PlayedRound subTop)
        {
            float diff = 100 - subTop.Points / (float)round.Points * 100;
            rate.scoresoustop = diff / sousTopRatioDiv;
            rate.scoreAll += rate.scoresoustop;
        }


        private int CompteRaccord(string word)
        {
            GadDagSearch search = new GadDagSearch(currentGame.Dico.Root);
            var res = search.SearchByPrefix(word, 1);
            res.AddRange(search.SearchBySuffix(word, 1));
            return res.Count;
        }
    }
}
