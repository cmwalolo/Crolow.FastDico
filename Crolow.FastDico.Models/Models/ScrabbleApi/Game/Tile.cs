using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;
using Newtonsoft.Json;

namespace Crolow.FastDico.Common.Models.ScrabbleApi.Game;

// *******************************
// Do never Move this to a class ! 
// *******************************

public struct Tile
{
    public Tile()
    {
    }

    public Tile(ITileConfig tile, Square parent)
    {
        Letter = tile.Letter;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
    }

    public Tile(Tile tile, Square parent)
    {
        Letter = tile.Letter;
        Points = tile.Points;
        IsJoker = tile.IsJoker;
        Parent = tile.Parent;
    }


    [JsonIgnore]
    public Square Parent { get; set; }
    public int PivotPoints { get; set; }
    public int Source { get; set; }

    public byte Letter { get; set; }
    public int Points { get; set; }
    public bool IsJoker { get; set; }
}
