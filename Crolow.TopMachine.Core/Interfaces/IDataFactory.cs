using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.TopMachine.Core.Interfaces;
public interface IDataFactory
{
    BoardConfigDataManager<BoardGrid> Boards { get; }
    GameConfigDataManager<GameConfig> Games { get; }

}