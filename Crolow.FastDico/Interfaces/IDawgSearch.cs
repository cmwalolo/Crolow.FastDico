using Crolow.Fast.Dawg.Dicos;

namespace Crolow.Fast.Dawg.Interfaces;

public interface IDawgSearch
{
    DawgNode Root { get; }
    List<string> SearchByPattern(string pattern);
    List<string> SearchByPrefix(string prefix);
    List<string> SearchBySuffix(string suffix);
    List<string> FindAllWordsFromLetters(string pattern);
    List<string> FindAllWordsContainingLetters(string pattern);
    bool SearchWord(string word);
}
