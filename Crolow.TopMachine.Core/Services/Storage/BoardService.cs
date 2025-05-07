using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Interfaces;

namespace Crolow.Pix.Core.Services.Storage
{
    public class BoardService : IBoardService
    {
        public IDataFactory dataFactory;

        public BoardService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<BoardGridModel> LoadAll()
        {
            return dataFactory.Boards.GetAllNodes().Result.ToList();
        }

        public void Update(BoardGridModel boardGrid)
        {
            dataFactory.Boards.Update(boardGrid);
            boardGrid.EditState = EditState.Unchanged;
        }

    }
}
