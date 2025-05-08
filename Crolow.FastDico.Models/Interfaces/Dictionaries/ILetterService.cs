using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Interfaces.Dictionaries
{
    public interface ILetterService
    {
        List<ILetterConfigModel> LoadAll();
        void Update(ILetterConfigModel gameConfig);
    }
}