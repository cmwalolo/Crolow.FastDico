using Crolow.FastDico.Dicos;

namespace Crolow.FastDico.Interfaces;

public interface IDawgSearch
{
    ILetterNode Root { get; }
    List<string> SearchByPattern(string pattern);
    List<string> SearchByPrefix(string prefix, int maxLength = int.MaxValue);
    List<string> SearchBySuffix(string suffix, int maxLength = int.MaxValue);
    List<string> FindAllWordsFromLetters(string pattern);
    List<string> FindAllWordsContainingLetters(string pattern);
    bool SearchWord(string word);
}
