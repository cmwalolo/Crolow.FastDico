using Crolow.FastDico.Models.ScrabbleApi.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface IGameConfigService
    {
        List<GameConfigModel> LoadAll();
        void Update(GameConfigModel gameConfig);
    }
}