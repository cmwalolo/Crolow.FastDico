using Crolow.FastDico.Models.Models.Dictionary.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface IDictionaryService
    {
        List<DictionaryModel> LoadAll();
        void Update(DictionaryModel album);
        public List<WordEntryModel> GetDefinitions(string word);
    }
}