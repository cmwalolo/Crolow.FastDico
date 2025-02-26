using Crolow.Fast.Dawg.Base;
using Crolow.Fast.Dawg.Dicos;
using Crolow.Fast.Dawg.Utils;

namespace Crolow.Fast.Dawg.Dawg;

public class DawgCompiler : BaseCompiler
{
    public override void Insert(string word)
    {
        var chars = DawgUtils.ConvertWordToBytes(word);
        var currentNode = RootBuild;

        foreach (var letter in chars)
        {
            DawgNodeBuild childNode = currentNode.Children.FirstOrDefault(c => c.Letter == letter);

            if (childNode == null)
            {
                childNode = new DawgNodeBuild { Id = BuildNodeId++, Letter = letter };
                currentNode.Children.Add(childNode);
            }
            currentNode = childNode;
        }
        currentNode.SetEnd();
    }
}
