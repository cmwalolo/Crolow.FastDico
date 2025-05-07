using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Common.Interfaces
{
    public interface IBoardService
    {
        List<BoardGridModel> LoadAll();
        void Update(BoardGridModel boardGrid);
    }
}