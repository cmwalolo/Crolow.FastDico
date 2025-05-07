using Crolow.FastDico.Common.Models.Dictionary.Entities;

namespace Crolow.FastDico.Common.Interfaces.Dictionaries
{
    public interface IDictionaryService
    {
        IBaseDictionary LoadDictionary(DictionaryModel model, string path);
        List<DictionaryModel> LoadAll();
        void Update(DictionaryModel album);
        public List<WordEntryModel> GetDefinitions(string word);
    }
}