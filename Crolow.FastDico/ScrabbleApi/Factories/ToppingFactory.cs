using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Components.Rounds;
using Crolow.FastDico.ScrabbleApi.Utils;

namespace Crolow.FastDico.ScrabbleApi.Factories
{
    public class ToppingFactory : IToppingFactory
    {
        CurrentGame CurrentGame { get; set; }

        public CurrentGame CreateGame(ToppingConfigurationContainer container)
        {
            var playConfiguration = new ConfigLoader().ReadConfiguration(container);
            this.CurrentGame = new CurrentGame();
            this.CurrentGame.Configuration = playConfiguration;

            CurrentGame.GameConfig = playConfiguration.SelectedConfig;
            GadDagDictionary gaddag = new GadDagDictionary();
            gaddag.ReadFromFile(container.Dictionary.DictionaryFile);

            CurrentGame.Board = new Board(this.CurrentGame);
            CurrentGame.LetterBag = new LetterBag(this.CurrentGame);
            CurrentGame.Rack = new PlayerRack();
            CurrentGame.Dico = gaddag;
            CurrentGame.Searcher = new GadDagSearch(CurrentGame.Dico.Root);

            CurrentGame.PivotBuilder = new PivotBuilder(CurrentGame);
            CurrentGame.BoardSolver = new BoardSolver(CurrentGame);

            if (CurrentGame.GameConfig.DifficultMode)
            {
                CurrentGame.Validator = new XRoundValidator(CurrentGame);
            }
            else
            {
                CurrentGame.Validator = new BaseRoundValidator(CurrentGame);
            }

            CurrentGame.ScrabbleEngine = new ScrabbleAI(CurrentGame);
            return CurrentGame;
        }
    }
}
