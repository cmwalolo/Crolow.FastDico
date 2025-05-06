using Crolow.FastDico.Models.ScrabbleApi.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface IBoardService
    {
        List<BoardGridModel> LoadAll();
        void Update(BoardGridModel boardGrid);
    }
}