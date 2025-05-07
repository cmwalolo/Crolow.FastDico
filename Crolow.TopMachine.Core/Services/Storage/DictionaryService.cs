using Crolow.FastDico.Base;
using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Models.Dictionary.Entities;
using Crolow.FastDico.GadDag;
using Crolow.TopMachine.Data.Interfaces;

namespace Crolow.TopMachine.Core.Services.Storage
{
    public class DictionaryService : IDictionaryService
    {
        public IDataFactory dataFactory;

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
            dataFactory.Dictionaries.Update(album);
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

        public IBaseDictionary LoadDictionary(DictionaryModel model, string path)
        {
            // Define the target path in AppDataDirectory
            string targetPath = Path.Combine(path, model.DictionaryFile);
            var dico = new GadDagDictionary();
            using (FileStream fileStream = new FileStream(targetPath, FileMode.Open))
                dico.ReadFromStream(fileStream);
            return dico;
        }
    }
}
