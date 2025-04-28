using Crolow.FastDico.Dicos;
using System.Runtime.InteropServices.Marshalling;

namespace Crolow.FastDico.Interfaces;

public interface IDawgSearch
{
    LetterNode Root { get; }
    List<string> SearchByPattern(string pattern);
    List<string> SearchByPrefix(string prefix, int maxLength = int.MaxValue);
    List<string> SearchBySuffix(string suffix, int maxLength = int.MaxValue);
    List<string> FindAllWordsFromLetters(string pattern);
    List<string> FindAllWordsContainingLetters(string pattern);
    bool SearchWord(string word);
}
