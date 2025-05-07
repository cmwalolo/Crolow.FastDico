using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Dicos;

namespace Crolow.FastDico.Utils
{
    public class GadDagUtils
    {
        public static LetterNode SearchWord(ILetterNode root, string word)
        {
            var bytes = TilesUtils.ConvertWordToBytes(word);
            return null; // SearchWordRecursive(root, bytes, 0, false);
        }

        private static ILetterNode SearchWordRecursive(ILetterNode currentNode, List<byte> word, int index, bool pastPivot)
        {
            if (index == word.Count)
            {
                return currentNode.IsEnd ? currentNode : null;
            }

            byte currentByte = word[index];

            // Traverse children to find the matching letter
            foreach (var child in currentNode.Children)
            {
                if (child.Letter == currentByte || !pastPivot && child.Letter == TilesUtils.PivotByte) // Pivot handling
                {
                    var node = SearchWordRecursive(child, word, index + 1, pastPivot || child.Letter == TilesUtils.PivotByte);
                    if (node != null)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        public static List<KeyValuePair<int, string>> FindPlusOne(ILetterNode rootNode, string word)
        {
            var bytes = TilesUtils.ConvertWordToBytes(word);
            var node = SearchWordRecursive(rootNode, bytes, 0, false);
            var result = new List<KeyValuePair<int, string>>();

            // Find one letter after the word
            foreach (var child in node.Children.Where(p => p.IsEnd))
            {
                var letter = TilesUtils.ConvertBytesToWord((new[] { child.Letter }).ToList());
                result.Add(new KeyValuePair<int, string>(1, letter));
            }

            // Find one letter before the word
            node = node.Children.FirstOrDefault(p => p.IsPivot);
            if (node != null)
            {
                foreach (var child in node.Children.Where(p => p.IsEnd))
                {
                    var letter = TilesUtils.ConvertBytesToWord((new[] { child.Letter }).ToList());
                    result.Add(new KeyValuePair<int, string>(0, letter));
                }
            }


            return result;
        }


    }
}
