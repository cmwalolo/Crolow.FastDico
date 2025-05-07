using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.FastDico.Common.Models.ScrabbleApi;

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
    public GameConfigModel GameConfig;
    public IDawgSearch Gaddag;
    public IPivotBuilder PivotBuilder;

}
