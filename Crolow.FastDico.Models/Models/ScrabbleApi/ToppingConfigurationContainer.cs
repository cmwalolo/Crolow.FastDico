using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Common.Models.ScrabbleApi;


public class ToppingConfigurationContainer
{
    public ToppingConfigurationContainer(GameConfigModel game, BoardGridModel board, LetterConfigModel letter, DictionaryModel dico)
    {
        GameConfig = game;
        BoardGrid = board;
        LetterConfig = letter;
        Dictionary = dico;
    }
    public ToppingConfigurationContainer() { }
    public GameConfigModel GameConfig { get; set; }
    public BoardGridModel BoardGrid { get; set; }
    public LetterConfigModel LetterConfig { get; set; }
    public DictionaryModel Dictionary { get; set; }
    public bool IsValid { get; set; }
}
