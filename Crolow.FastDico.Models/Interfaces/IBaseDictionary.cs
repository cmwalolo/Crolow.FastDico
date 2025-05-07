using Crolow.FastDico.Dicos;

namespace Crolow.FastDico.Base
{
    public interface IBaseDictionary
    {
        int BuildNodeId { get; set; }
        ILetterNode Root { get; }
        ILetterNode RootBuild { get; }

        void Build(IEnumerable<string> words);
        void Insert(string word);
        void ReadFromFile(string filePath);
        void SaveToFile(string filePath);
        void ReadFromStream(Stream stream);

    }
}