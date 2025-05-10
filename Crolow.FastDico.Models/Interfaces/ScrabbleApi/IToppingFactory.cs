using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;

namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{
    public interface IToppingFactory
    {
        CurrentGame CreateGame(ToppingConfigurationContainer container, IBaseDictionary dico);
        CurrentGame CreateGame(ToppingConfigurationContainer container, IDictionaryService dicoService, string path);

    }
}