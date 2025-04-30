using Crolow.FastDico.Dawg;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolvers;
using Crolow.FastDico.ScrabbleApi.Config;

namespace Crolow.FastDico.ScrabbleApi.GameObjects;

public class CurrentGame
{
    public CurrentGame()
    {
        RoundsPlayed = new List<PlayedRound>();
    }

    public PlayConfiguration Configuration { get; set; }

    public int Round;
    public List<PlayedRound> RoundsPlayed;
    public int TotalPoints;
    public int PlayTime;
    public LetterBag LetterBag;
    public PlayerRack Rack;

    public GadDagCompiler Dico;
    public PlayConfiguration playConfiguration;
    public Board Board;
    public GameConfigModel GameConfig;
    public DawgSearch Gaddag;
    public PivotBuilder PivotBuilder;

}
