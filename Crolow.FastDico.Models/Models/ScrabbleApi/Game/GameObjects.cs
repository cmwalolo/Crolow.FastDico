using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Models.ScrabbleApi.Game;

public class CurrentGame
{
    public GameObjects GameObjects { get; set; }
    public GameControllersSetup ControllersSetup { get; set; }
}

public class GameObjects
{
    public GameObjects()
    {
        RoundsPlayed = new List<PlayableSolution>();
    }
    public PlayConfiguration Configuration { get; set; }
    public int Round { get; set; }
    public List<PlayableSolution> RoundsPlayed { get; set; }
    public int TotalPoints { get; set; }
    public int PlayTime { get; set; }
    public LetterBag LetterBag { get; set; }
    public PlayerRack Rack { get; set; }
    public Board Board { get; set; }
    public GameStatus GameStatus { get; set; }
    public IGameConfigModel GameConfig { get; set; }
    public ILetterConfigModel LetterConfig { get; set; }
    public PlayableSolution SelectedRound { get; set; }
}

public class GameControllersSetup
{
    public IBaseDictionary Dico;
    public IDawgSearch Searcher;
    public IPivotBuilder PivotBuilder;
    public IBoardSolver BoardSolver;
    public IBaseRoundValidator Validator;
    public IScrabbleAI ScrabbleEngine;
}
public enum GameStatus
{
    None = 0,
    Initialized = 1,
    WaitingToStart = 3,
    WaitingForRack = 4,
    WaitingForNextRound = 5,
    GameEnded = 6,
}
