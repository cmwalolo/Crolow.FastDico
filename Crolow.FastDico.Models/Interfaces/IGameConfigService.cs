using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Common.Interfaces
{
    public interface IGameConfigService
    {
        List<GameConfigModel> LoadAll();
        void Update(GameConfigModel gameConfig);
    }
}