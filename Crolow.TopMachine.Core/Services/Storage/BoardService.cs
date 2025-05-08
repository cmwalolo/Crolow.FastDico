using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.Pix.Core.Services.Storage
{
    public class BoardService : IBoardService
    {
        public IDataFactory dataFactory;

        public BoardService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<IBoardGridModel> LoadAll()
        {
            return dataFactory.Boards.GetAllNodes().Result.ToList();
        }

        public void Update(IBoardGridModel boardGrid)
        {
            dataFactory.Boards.Update(boardGrid);
            boardGrid.EditState = EditState.Unchanged;
        }

    }
}
