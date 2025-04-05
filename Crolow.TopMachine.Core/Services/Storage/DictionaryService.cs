using Crolow.FastDico.Models.Models.Dictionary.Entities;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data.Interfaces;
using Crolow.TopMachine.Data.Repositories;

namespace Crolow.Pix.Core.Services.Storage
{
    public class DictionaryService : IDictionaryService
    {
        public IDataFactory dataFactory;
        public DictionaryDataManager<DictionaryModel> dicoRepository;

        public DictionaryService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<DictionaryModel> LoadAll()
        {
            return dataFactory.Dictionaries.GetAllNodes().Result.ToList();
        }



        public void Update(DictionaryModel album)
        {
            dicoRepository.Update(album);
            album.EditState = EditState.Unchanged;
        }

        public List<WordEntryModel> GetDefinitions(string word)
        {
            var words = dataFactory.DicoWords.GetAllNodes(p => p.Word == word.ToLower());
            var defs = new List<WordEntryModel>();
            foreach (var wordEntry in words)
            {
                defs.AddRange(dataFactory.DicoEntries.GetAllNodes(p => p.Id == wordEntry.Parent));
            }

            return defs.ToList();
        }
    }
}
