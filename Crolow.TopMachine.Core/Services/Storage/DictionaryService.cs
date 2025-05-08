using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.GadDag;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities.Definitions;

namespace Crolow.TopMachine.Core.Services.Storage
{
    public class DictionaryService : IDictionaryService
    {
        public IDataFactory dataFactory;

        public DictionaryService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<IDictionaryModel> LoadAll()
        {
            return dataFactory.Dictionaries.GetAllNodes().Result.ToList();
        }



        public void Update(IDictionaryModel album)
        {
            dataFactory.Dictionaries.Update(album);
            album.EditState = EditState.Unchanged;
        }

        public List<IWordEntryModel> GetDefinitions(string word)
        {
            var words = dataFactory.DicoWords.GetAllNodes(p => p.Word == word.ToLower());
            var defs = new List<IWordEntryModel>();
            foreach (var wordEntry in words)
            {
                defs.AddRange(dataFactory.DicoEntries.GetAllNodes(p => p.Id == wordEntry.Parent));
            }

            return defs.ToList();
        }

        public IBaseDictionary LoadDictionary(IDictionaryModel model, string path)
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
