using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Interfaces
{
    public interface IGameConfigService
    {
        List<IGameConfigModel> LoadAll();
        void Update(IGameConfigModel gameConfig);
    }
}