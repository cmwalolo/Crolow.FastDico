using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Models.ScrabbleApi.Game;

public class PlayedRounds
{
    public IGameConfigModel Config { get; set; }
    public int MaxPoints { get; set; }
    public int MaxSubTopPoints { get; set; }
    public bool PickAll { get; set; }

    public List<PlayableSolution> Tops { get; set; }
    public List<PlayableSolution> SubTops { get; set; }
    public List<PlayableSolution> AllRounds { get; set; }
    public PlayableSolution CurrentRound { get; set; }

    public PlayerRack PlayerRack { get; set; }

    public PlayedRounds(IGameConfigModel config, List<Tile> rack, bool pickAll)
    {
        Config = config;
        Tops = new List<PlayableSolution>();
        SubTops = new List<PlayableSolution>();
        AllRounds = new List<PlayableSolution>();
        CurrentRound = new PlayableSolution();
        PlayerRack = new PlayerRack(rack);
        PickAll = pickAll;
    }
}
