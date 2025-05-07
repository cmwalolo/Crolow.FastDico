using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Common.Interfaces.Dictionaries
{
    public interface ILetterService
    {
        List<LetterConfigModel> LoadAll();
        void Update(LetterConfigModel gameConfig);
    }
}