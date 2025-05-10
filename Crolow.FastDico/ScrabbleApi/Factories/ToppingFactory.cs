using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Components.Rounds;
using Crolow.FastDico.ScrabbleApi.Utils;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.ScrabbleApi.Factories
{
    public class ToppingFactory : IToppingFactory
    {
        public CurrentGame CreateGame(ToppingConfigurationContainer container, IDictionaryService dicoService, string path)

        {
            var dico = dicoService.LoadDictionary(container.Dictionary, path);
            return CreateGame(container, dico);
        }

        public CurrentGame CreateGame(ToppingConfigurationContainer container, IBaseDictionary dico)
        {
            var playConfiguration = new ConfigLoader().ReadConfiguration(container);

            TilesUtils.configuration = playConfiguration.BagConfig;

            var CurrentGame = new CurrentGame
            {
                ControllersSetup = new GameControllersSetup(),
                GameObjects = new GameObjects()
            };

            CurrentGame.GameObjects.Configuration = playConfiguration;
            CurrentGame.GameObjects.GameConfig = playConfiguration.SelectedConfig;
            CurrentGame.GameObjects.LetterConfig = container.LetterConfig;
            CurrentGame.ControllersSetup.Dico = dico;

            CurrentGame.GameObjects.Board = new Board(CurrentGame.GameObjects);
            CurrentGame.GameObjects.LetterBag = new LetterBag(CurrentGame.GameObjects);
            CurrentGame.GameObjects.Rack = new PlayerRack();
            CurrentGame.ControllersSetup.Dico = dico;
            CurrentGame.ControllersSetup.Searcher = new GadDagSearch(CurrentGame.ControllersSetup.Dico.Root);

            CurrentGame.ControllersSetup.PivotBuilder = new PivotBuilder(CurrentGame);
            CurrentGame.ControllersSetup.BoardSolver = new BoardSolver(CurrentGame);

            if (CurrentGame.GameObjects.GameConfig.DifficultMode)
            {
                CurrentGame.ControllersSetup.Validator = new XRoundValidator(CurrentGame);
            }
            else
            {
                CurrentGame.ControllersSetup.Validator = new BaseRoundValidator(CurrentGame);
            }

            CurrentGame.ControllersSetup.ScrabbleEngine = new ScrabbleAI(CurrentGame);
            CurrentGame.GameObjects.GameStatus = GameStatus.WaitingToStart;
            return CurrentGame;
        }
    }
}
