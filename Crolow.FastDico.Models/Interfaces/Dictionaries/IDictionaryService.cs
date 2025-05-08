using Crolow.TopMachine.Data.Bridge.Entities.Definitions;

namespace Crolow.FastDico.Common.Interfaces.Dictionaries
{
    public interface IDictionaryService
    {
        IBaseDictionary LoadDictionary(IDictionaryModel model, string path);
        List<IDictionaryModel> LoadAll();
        void Update(IDictionaryModel album);
        public List<IWordEntryModel> GetDefinitions(string word);
    }
}