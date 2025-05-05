using Kalow.Apps.Common.DataTypes;
using Kalow.Hypergram.Core.Services;
using Kalow.Hypergram.Core.Solver.Utils;
using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;
using Kalow.Hypergram.Logic.Solver;
using MauiBlazorWeb.Shared.Interfaces.HyperGram;

namespace MauiBlazorWeb.Shared.Services.Hypergram
{

    public class HypergramGameService : IHypergramGameService
    {
        protected readonly HypergramBoardConfigService boardConfigService;
        protected readonly ResourceReaderService resourceReaderService;
        //protected readonly DicoUtils dicoUtils;

        private LocalSolver solver { get; set; }

        public HypergramGameService(/*DicoUtils dicoUtils*/ HypergramBoardConfigService configService, ResourceReaderService resourceReaderService)
        {
            this.boardConfigService = configService;
            this.resourceReaderService = resourceReaderService;
            //this.dicoUtils = dicoUtils;

        }

        /// <summary>
        /// This function creates a new empty room for a robot play
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isRobotPlay"></param>
        public void SetNewGame(HypergramConfig config, bool isRobotPlay)
        {
            HypergramContext.CurrentPlayer = new HypergramPlayer()
            {
                Id = KalowId.NewObjectId(), // - HypergramContext.CurrentUser.User.Id,
                Name = HypergramContext.CurrentUser.User.Name,
                CurrentRack = new HypergramWordContainer(),
                LastRack = new HypergramWordContainer(),
            };

            var RobotPlayer = new HypergramPlayer()
            {
                Id = KalowId.Empty,
                Name = "Robot",
                IsRobot = true,
                CurrentRack = new HypergramWordContainer(),
                LastRack = new HypergramWordContainer(),
            };


            var room = new HypergramRoom
            {
                Owner = HypergramContext.CurrentUser.User,
                GameStatus = HypergramRoom.RoomStatus.Empty,
                IsRobotGame = true
            };

            var stringBuilder = resourceReaderService.GetBoardConfigServiceStream();
            room.Board.BoardConfig = boardConfigService.Load("FR", stringBuilder);

            room.Board.Config = config;
            if (System.Math.Floor((double)Random.Shared.Next(200) / 100) == 1)
            {
                room.Board.PlayerBoards.Add(HypergramContext.CurrentPlayer);
                room.Board.PlayerBoards.Add(RobotPlayer);
            }
            else
            {
                room.Board.PlayerBoards.Add(RobotPlayer);
                room.Board.PlayerBoards.Add(HypergramContext.CurrentPlayer);
            }

            HypergramContext.CurrentPlayer.TimeUsed = 0;
            HypergramContext.CurrentPlayer.TotalPoints = 0;

            HypergramContext.CurrentRoom = room;

            room.Board.GameRacks = new List<HypergramWordContainer>();
            for (int x = 0; x < room.Board.Config.NumberOfRacks; x++)
            {
                room.Board.GameRacks.Add(new HypergramWordContainer());
            }

            room.Board.GameBag = new HypergramBag(room.Board.BoardConfig, room.Board.Config);
            PrepareGame();
        }

        /// <summary>
        /// This function creates a new empty room for a robot play
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isRobotPlay"></param>
        public void PrepareGame()
        {
            var room = HypergramContext.CurrentRoom;
            //var dico = dicoUtils.SetDicoUtils("FR", resourceReaderService.GetDawgStream());
            solver = new LocalSolver(room.Board, dico);
            solver.InitializeGame();
            HypergramContext.CurrentRoom.GameStatus = HypergramRoom.RoomStatus.WaitingForStart;
        }

        public void StartGame()
        {
            HypergramContext.CurrentRoom.GameStatus = HypergramRoom.RoomStatus.Started;
        }

        public HypergramGameRound PlayNextRound(HypergramPlayer targetPlayer, bool checkStolenRack)
        {
            var room = HypergramContext.CurrentRoom;

            var player = room.Board.PlayerBoards[room.Board.CurrentPlayer];

            if (player.IsRobot)
            {
                var result = solver.SolveRack(player, targetPlayer);
                return result;
            }

            return null;
        }

        public void Pick(HypergramPlayer targetPlayer)
        {
            var room = HypergramContext.CurrentRoom;
            var player = room.Board.PlayerBoards[room.Board.CurrentPlayer];

            if (player.Id == targetPlayer.Id)
            {
                player.NextPickAmount = player.CurrentRack.WordLength;
                //player.LastRack.SetNewWord(player.CurrentRack.GetWord());
                player.IsEnabled = true;
                player.CanPick = false;
                player.CanChange = false;
                player.CanPass = true;
                player.IsCurrent = true;
                player.IsEnabled = true;
                player.CanPlay = true;
                CreatePlayerRack(false);
            }
            else
            {
                player.IsEnabled = false;
                targetPlayer.IsCurrent = false;
                targetPlayer.IsEnabled = true;
            }
            return;
        }

        public void Change()
        {
            var room = HypergramContext.CurrentRoom;
            var player = room.Board.PlayerBoards[room.Board.CurrentPlayer];
            player.NextPickAmount = player.CurrentRack.WordLength;

            // We override LastRack And CurrentRack 
            // So we take the current amount of letters
            player.LastRack.SetNewWord(player.CurrentRack.GetWord());
            player.CurrentRack.SetNewWord("");
            player.IsEnabled = true;
            player.CanPick = false;
            player.CanChange = false;
            player.CanPass = true;
            player.CanPlay = true;
            CreatePlayerRack(true);
            return;
        }

        public HypergramGameRound Pass()
        {
            var room = HypergramContext.CurrentRoom;
            var player = room.Board.PlayerBoards[room.Board.CurrentPlayer];
            player.IsEnabled = false;
            player.CanPick = true;
            player.CanChange = true;
            player.CanPass = true;

            room.Board.TotalRounds++;
            HypergramGameRound round = new HypergramGameRound()
            {
                PlayerId = player.Id,
                TargetPlayerId = player.Id,
                HasPassed = true,
                RemainingRack = player.CurrentRack.GetWord(),
                Turn = room.Board.TotalRounds
            };

            return round;
        }

        public void SetNextPlayer()
        {
            SelectNextPlayer();
            CheckEndOfGame();
        }

        public bool CheckWord(string word)
        {
            return solver.CheckWord(word);
        }

        public HypergramGameRound SetPlayedRound(HypergramPlayer selectedPlayer, HypergramGameRound round)
        {

            var room = HypergramContext.CurrentRoom;
            var player = room.Board.PlayerBoards[room.Board.CurrentPlayer];

            solver.SetPlayedRound(player, selectedPlayer, round);
            solver.CheckBoard();

            if (round.Points > 0)
            {
                player.IsEnabled = false;
            }
            return round;
        }


        #region private functions
        private void CreatePlayerRack(bool change)
        {
            var room = HypergramContext.CurrentRoom;
            var player = room.Board.PlayerBoards[room.Board.CurrentPlayer];

            if (change)
            {
                player.NextPickAmount = player.LastRack.WordLength;
            }
            else
            {
                if (room.Board.Config.PickupBonusSkip && player.LastRack.WordLength == room.Board.Config.MaxPlayerRackLength)
                {
                    player.NextPickAmount = room.Board.Config.PickupBonus;
                }
                else
                {
                    var len = player.LastRack.WordLength - player.CurrentRack.WordLength;
                    player.NextPickAmount = Math.Min(room.Board.Config.MaxPickupLength, len) + room.Board.Config.PickupBonus;
                }

                if (player.NextPickAmount + player.CurrentRack.WordLength > room.Board.Config.MaxPlayerRackLength)
                {
                    player.NextPickAmount = room.Board.Config.MaxPlayerRackLength - player.CurrentRack.WordLength;
                }
            }

            if (!solver.SetupRack(player))
            {
                room.Board.LastRound = true;
            }
        }

        private void CheckEndOfGame()
        {
            if (HypergramContext.CurrentRoom.GameStatus == HypergramRoom.RoomStatus.Finished)
            {
                foreach (var player in HypergramContext.CurrentRoom.Board.PlayerBoards)
                {
                    player.IsEnabled = false;
                    player.CanChange = false;
                    player.CanPick = false;
                }
            }
        }

        private void SelectNextPlayer()
        {
            if (HypergramContext.CurrentRoom.Board.TotalRounds > 0)
            {
                HypergramContext.CurrentRoom.Board.CurrentPlayer++;
            }

            if (HypergramContext.CurrentRoom.Board.CurrentPlayer >= HypergramContext.CurrentRoom.Board.PlayerBoards.Count)
            {
                if (HypergramContext.CurrentRoom.Board.LastRound)
                {
                    HypergramContext.CurrentRoom.GameStatus = HypergramRoom.RoomStatus.Finished;
                    return;
                }
                HypergramContext.CurrentRoom.Board.CurrentPlayer = 0;
            }

            var player = HypergramContext.CurrentRoom.Board.PlayerBoards[HypergramContext.CurrentRoom.Board.CurrentPlayer];

            if (HypergramContext.CurrentRoom.Board.TotalRounds == 0)
            {
                player.IsEnabled = true;
                player.CanPick = true;
                player.CanPass = false;
                player.CanPlay = false;
            }
            else
            {
                player.IsEnabled = true;
                player.CanPick = true;
                player.CanPass = false;
                player.CanPlay = false;

            }

            player.IsCurrent = true;

        }
        #endregion
    }
}

