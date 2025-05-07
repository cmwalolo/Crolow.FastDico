using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;

namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{
    public interface IToppingFactory
    {
        CurrentGame CreateGame(ToppingConfigurationContainer container);
    }
}