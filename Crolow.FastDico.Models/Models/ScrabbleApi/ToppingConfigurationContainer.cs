using Crolow.TopMachine.Data.Bridge.Entities.Definitions;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Models.ScrabbleApi;


public class ToppingConfigurationContainer
{
    public ToppingConfigurationContainer(IGameConfigModel game, IBoardGridModel board, ILetterConfigModel letter, IDictionaryModel dico)
    {
        GameConfig = game;
        BoardGrid = board;
        LetterConfig = letter;
        Dictionary = dico;
    }
    public ToppingConfigurationContainer() { }
    public IGameConfigModel GameConfig { get; set; }
    public IBoardGridModel BoardGrid { get; set; }
    public ILetterConfigModel LetterConfig { get; set; }
    public IDictionaryModel Dictionary { get; set; }
    public bool IsValid { get; set; }
}
