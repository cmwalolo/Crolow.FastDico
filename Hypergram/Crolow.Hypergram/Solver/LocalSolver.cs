using Kalow.Hypergram.Core.Dawg;
using Kalow.Hypergram.Core.Solver.Utils;
using Kalow.Hypergram.Logic.Models.GamePlay;

namespace Kalow.Hypergram.Logic.Solver
{
    public class LocalSolver(HypergramBoard board/*, dawgDictionary dico*/)
    {
        protected HypergramBoard board = board;
        //        protected dawgDictionary dico = dico;
        protected Dictionary<string, int> cacheExtensions = new();

        /// <summary>
        /// Initialize the board with random words
        /// </summary>
        public void InitializeGame()
        {
            foreach (var container in board.GameRacks)
            {
                LoadBoardRack(container);
            }
        }

        private void LoadBoardRack(HypergramWordContainer container)
        {
            var boardRack = new HypergramRack(board.BoardConfig);
            var playerRack = new HypergramRack(board.BoardConfig);

            bool busy = true;

            if (board.Config.StartRackLength > 0)
            {
                for (int x = 0; x < board.Config.StartRackLength; x++)
                {
                    playerRack.Add(board.GameBag.SelectRandom());
                }

                var results = new List<HypergramRound>();

                while (busy)
                {
                    busy = false;
                    uint n = 0 // dico.Root();
                    var currentRound = new HypergramRound(board.BoardConfig);

                    FindSolutions(boardRack, playerRack, results, n, currentRound);

                    // all tiles of the board rack need to be used and the length of the words need
                    // to be the one set in the config ... Which is the case
                    // here as the board rack is empty at initialization time... 
                    // I keep this for further reuse
                    // results = results.Where(p => p.wordlen() >= board.Config.StartRackLength && p.countfromboard() == len).ToList();

                    // If there is no result, we add a tile in the bag and start over again
                    if (results.Count == 0)
                    {
                        if (board.GameBag.Ntiles() > 0)
                        {
                            playerRack.Add(board.GameBag.SelectRandom());
                            busy = true;
                        }
                    }
                }

                // when result is selected unused tiles are put back in the BAG.
                // As we are in the initialization and there is no effect play bag.
                if (results.Count > 0)
                {
                    var selectedResult = results[new Random().Next(results.Count)];
                    for (int x = 0; x < selectedResult.WordLen(); x++)
                    {
                        if (selectedResult.Joker(x) == DicoConstants.JOKER)
                        {
                            board.GameBag.TakeTile(DicoConstants.JOKER_TILE);
                        }
                        else if (selectedResult.GetTileOrigin(x) == 2)
                        {
                            var tile = selectedResult.GetTile(x);
                            board.GameBag.TakeTile(tile);
                        }
                    }

                    string word = selectedResult.GetWord();
                    container.SetNewWord(word, selectedResult.GetPoints());
                }
            }
            else
            {
                container.SetNewWord(string.Empty, 0);
            }
        }

        protected void FindSolutions(HypergramRack boardRack, HypergramRack playerRack,
                        List<HypergramRound> rounds, uint n, HypergramRound partialRound)
        {
            char l;
            uint succ;

            if (dico.Word(n) > 0)
            {
                var round = new HypergramRound(board.BoardConfig);

                // all tiles of the board rack need to be used and the length of the words need
                // to be the one set in the config ... Which is the case
                // here as the board rack is empty at initialization time... 
                // I keep this for further reuse
                if (partialRound.WordLen() <= board.Config.BoardLength && partialRound.WordLen() >= board.Config.StartRackLength && partialRound.CountFromRack() > 0 && partialRound.CountFromRack() <= board.Config.MaxLetterPlayed && boardRack.Ntiles() == 0)
                {
                    round.Copy(partialRound.Round);
                    round.SetPoints();
                    rounds.Add(round);
                }
            }

            for (succ = dico.Succ(n); succ > 0; succ = dico.Next(succ))
            {
                l = dico.Chr(succ);

                if (boardRack.IsIn(l) > 0)
                {
                    boardRack.Remove(l);
                    partialRound.AddRightFromBoard(l);
                    FindSolutions(boardRack, playerRack, rounds, succ, partialRound);
                    partialRound.RemoveRightToBoard();
                    boardRack.Add(l);
                }
                else
                {
                    if (playerRack.IsIn(l) > 0)
                    {
                        playerRack.Remove(l);
                        partialRound.AddRightFromRack(l, 0);
                        FindSolutions(boardRack, playerRack, rounds, succ, partialRound);
                        partialRound.RemoveRightToRack();
                        playerRack.Add(l);
                    }
                    else
                    {
                        if (playerRack.IsIn(DicoConstants.JOKER_TILE) > 0)
                        {
                            playerRack.Remove(DicoConstants.JOKER_TILE);
                            partialRound.AddRightFromRack(l, 1);
                            FindSolutions(boardRack, playerRack, rounds, succ, partialRound);
                            partialRound.RemoveRightToRack();
                            playerRack.Add(DicoConstants.JOKER_TILE);
                        }
                    }
                }
            }
        }

        private HypergramRack GetInternalRack(HypergramWordContainer wc)
        {
            var playerRack = new HypergramRack(board.BoardConfig);
            for (var x = 0; x < wc.WordLength; x++)
            {
                var tile = board.BoardConfig.GetTileCode(wc.IsJoker(x) ? (char)0 : wc.WordTiles[x]);
                playerRack.Add(tile);
            }
            playerRack.AlignTiles();
            return playerRack;
        }

        public bool SetupRack(HypergramPlayer player)
        {
            HypergramRack playerRack = GetInternalRack(player.CurrentRack);

            if (board.GameBag.Ntiles() > 0)
            {
                if (!player.Initialized)
                {
                    player.Initialized = true;
                    for (int x = 0; board.GameBag.Ntiles() > 0 && x < board.Config.StartPlayerRackLength; x++)
                    {
                        int tile = board.GameBag.SelectRandom();
                        playerRack.Add(tile);
                        board.GameBag.TakeTile(tile);
                        var tileChar = board.BoardConfig.GetTileAsciiChar(tile);
                        player.CurrentRack.AddToWord(tileChar);
                    }
                }
                else
                {
                    for (int x = 0; board.GameBag.Ntiles() > 0 && x < player.NextPickAmount; x++)
                    {
                        int tile = board.GameBag.SelectRandom();
                        playerRack.Add(tile);
                        board.GameBag.TakeTile(tile);
                        var tileChar = board.BoardConfig.GetTileAsciiChar(tile);
                        player.CurrentRack.AddToWord(tileChar);
                    }
                }
            }

            return board.GameBag.Ntiles() > 0;
        }

        public HypergramGameRound SolveRack(HypergramPlayer player, HypergramPlayer targetPlayer)
        {
            HypergramRack playerRack = GetInternalRack(targetPlayer.CurrentRack);
            var results = new List<HypergramRound>();
            int rack = 0;

            foreach (var container in board.GameRacks)
            {
                HypergramRack boardRack = GetInternalRack(container);
                uint n = dico.Root();
                var currentRound = new HypergramRound(board.BoardConfig, rack);
                FindSolutions(boardRack, playerRack, results, n, currentRound);
                rack++;
            }

            var selected = results.OrderByDescending(p => p.GetPoints() - board.GameRacks[p.GetRackPlayed()].GetScore());

            if (selected != null && selected.Any())
            {
                var oldRack = targetPlayer.CurrentRack.GetWord();
                var selectedResult = selected.First();
                string playedLetters = string.Empty;
                foreach (var result in selected)
                {
                    playerRack = GetInternalRack(targetPlayer.CurrentRack);
                    selectedResult = result;
                    playedLetters = string.Empty;
                    for (int x = 0; x < selectedResult.WordLen(); x++)
                    {
                        if (selectedResult.PlayedFromRack(x) == 2)
                        {
                            playerRack.Remove(selectedResult.GetTile(x));
                            playedLetters += board.BoardConfig.GetTileAsciiChar(selectedResult.GetTile(x));

                        }
                    }

                    if (oldRack.Length > 3 && board.Config.MaxPlayerRackLength > 3 && board.Config.MaxLetterPlayed > 3 && playedLetters.Length < 3 && playerRack.Ntiles() < board.Config.BoardLength - 2)
                    {
                        continue;
                    }

                    break;
                }

                // We dont make the move if too low letters
                // Even if the word is stolen we can still 
                // The Hypergram mode will skip to small moves 
                // So it will be able to change letters the next round
                if (!(oldRack.Length > 3 && board.Config.MaxPlayerRackLength > 3 && board.Config.MaxLetterPlayed > 3 && playedLetters.Length < 3 && playerRack.Ntiles() < board.Config.BoardLength - 2))
                {
                    playerRack.AlignTiles();

                    var newWord = selectedResult.GetWord();
                    var newPoints = selectedResult.GetPoints();
                    var rackPlayed = selectedResult.GetRackPlayed();

                    var container = board.GameRacks[rackPlayed];

                    // *** TODO *** Where is the multiplier ?
                    var oldPoints = container.GetScore();
                    newPoints = selectedResult.GetPoints();

                    if (oldRack.Length > playerRack.GetRackString().Length + playedLetters.Length)
                    {
                        Console.WriteLine("What the fuxk !!");
                    }
                    var round = new HypergramGameRound()
                    {
                        PlayerId = player.Id,
                        Points = newPoints - oldPoints,
                        Turn = board.TotalRounds,
                        RemainingRack = playerRack.GetRackString(),
                        WordPlayed = newWord,
                        LettersPlayed = playedLetters,
                        RackPlayed = rackPlayed,
                        PreviousWord = container.Word
                    };

                    board.LastPlayedRack = rackPlayed;

                    return round;
                }
            }


            return new HypergramGameRound()
            {
                PlayerId = player.Id,
                Points = 0,
                HasPassed = true,
                Turn = board.TotalRounds,
                RemainingRack = playerRack.GetRackString(),
                WordPlayed = string.Empty,
                LettersPlayed = string.Empty,
                RackPlayed = -1,
                PreviousWord = string.Empty
            };
        }

        public void CheckBoard()
        {
            foreach (var container in board.GameRacks)
            {
                var word = container.GetWord();
                if (board.TotalRounds - container.LastRound > board.Config.TurnOver || word.Length == board.Config.BoardLength)
                {
                    container.ClearContainer();
                    LoadBoardRack(container);
                    container.LastRound = board.TotalRounds;
                }
            }
        }

        public bool CheckWord(string word)
        {
            return dico.SearchWord(word) == 1;
        }

        public void SetPlayedRound(HypergramPlayer player, HypergramPlayer selectedPlayer, HypergramGameRound round)
        {
            board.TotalRounds++;
            player.TotalPoints += round.Points;
            round.Turn = board.TotalRounds;

            // The rack has been stoled, so we will keep the new rack as reference for the next round
            if (selectedPlayer.Id != player.Id)
            {
                selectedPlayer.LastRack.SetNewWord(round.RemainingRack);
            }
            else
            {
                selectedPlayer.LastRack.SetNewWord(selectedPlayer.CurrentRack.Word);
            }

            selectedPlayer.CurrentRack.SetNewWord(round.RemainingRack);

            if (!round.HasPassed)
            {
                board.GameRacks[round.RackPlayed].SetNewWord(round.WordPlayed, board.BoardConfig.GetTilePointsFromString(round.WordPlayed) * round.WordPlayed.Length);
                board.GameRacks[round.RackPlayed].LastRound = board.TotalRounds;
            }
            board.Rounds.Add(round);
        }
    }
}
