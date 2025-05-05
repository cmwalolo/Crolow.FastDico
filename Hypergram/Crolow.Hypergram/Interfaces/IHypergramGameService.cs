using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;

namespace MauiBlazorWeb.Shared.Interfaces.HyperGram
{
    public interface IHypergramGameService
    {
        void SetNewGame(HypergramConfig config, bool isRobotPlay);
        void PrepareGame();
        void StartGame();

        HypergramGameRound Pass();
        void Pick(HypergramPlayer targetPlayer);
        void Change();

        // Only for Robot Player
        HypergramGameRound PlayNextRound(HypergramPlayer targetPlayer, bool checkStolenRack);
        bool CheckWord(string word);
        HypergramGameRound SetPlayedRound(HypergramPlayer player, HypergramGameRound round);
        void SetNextPlayer();
    }
}