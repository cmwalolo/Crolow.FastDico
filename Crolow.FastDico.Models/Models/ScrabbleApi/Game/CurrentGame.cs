using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Models.ScrabbleApi.Game;

public class CurrentGame
{
    public CurrentGame()
    {
        RoundsPlayed = new List<PlayableSolution>();
    }

    public PlayConfiguration Configuration { get; set; }

    public int Round;
    public List<PlayableSolution> RoundsPlayed;
    public int TotalPoints;
    public int PlayTime;
    public LetterBag LetterBag;
    public PlayerRack Rack;

    public IBaseDictionary Dico;
    public PlayConfiguration playConfiguration;
    public Board Board;
    public IGameConfigModel GameConfig;
    public IDawgSearch Searcher;
    public IPivotBuilder PivotBuilder;
    public IBoardSolver BoardSolver;
    public IBaseRoundValidator Validator;
    public IScrabbleAI ScrabbleEngine;

}
