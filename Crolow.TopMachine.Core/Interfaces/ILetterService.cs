using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface ILetterService
    {
        List<LetterConfigModel> LoadAll();
        void Update(LetterConfigModel gameConfig);
    }
}