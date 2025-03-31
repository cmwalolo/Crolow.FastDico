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
            dicoRepository = dataFactory.Dictionaries;
        }

        public List<DictionaryModel> LoadAll()
        {
            return dicoRepository.GetAllNodes().ToList();
        }

        public void Update(DictionaryModel album)
        {
            dicoRepository.Update(album);
            album.EditState = EditState.Unchanged;
        }
    }
}
