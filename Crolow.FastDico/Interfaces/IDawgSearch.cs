using Crolow.FastDico.Dicos;

namespace Crolow.FastDico.Interfaces;

public interface IDawgSearch
{
    LetterNode Root { get; }
    List<string> SearchByPattern(string pattern);
    List<string> SearchByPrefix(string prefix);
    List<string> SearchBySuffix(string suffix);
    List<string> FindAllWordsFromLetters(string pattern);
    List<string> FindAllWordsContainingLetters(string pattern);
    bool SearchWord(string word);
}
