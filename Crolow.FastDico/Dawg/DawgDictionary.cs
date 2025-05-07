using Crolow.FastDico.Base;
using Crolow.FastDico.Dicos;
using Crolow.FastDico.Utils;

namespace Crolow.FastDico.Dawg;

public class DawgDictionary : BaseDictionary
{
    public override void Insert(string word)
    {
        var chars = TilesUtils.ConvertWordToBytes(word.ToUpper());
        var currentNode = RootBuild;

        foreach (var letter in chars)
        {
            ILetterNode childNode = currentNode.Children.FirstOrDefault(c => c.Letter == letter);

            if (childNode == null)
            {
                childNode = new LetterNode { Letter = letter };
                currentNode.Children.Add(childNode);
            }
            currentNode = childNode;
        }
        currentNode.SetEnd();
    }
}
