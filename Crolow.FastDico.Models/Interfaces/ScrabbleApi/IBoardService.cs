using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Interfaces.ScrabbleApi
{
    public interface IBoardService
    {
        List<IBoardGridModel> LoadAll();
        void Update(IBoardGridModel boardGrid);
    }
}