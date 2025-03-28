using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.TopMachine.Core
{
    public class DataFactory : IDataFactory
    {
        private DatabaseSettings settings;
        public DataFactory(DatabaseSettings settings)
        {
            this.settings = settings;
        }

        public BoardConfigDataManager<BoardGrid> Boards => new BoardConfigDataManager<BoardGrid>(settings);
        public GameConfigDataManager<GameConfig> Games => new GameConfigDataManager<GameConfig>(settings);
    }
}
