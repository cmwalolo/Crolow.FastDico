using HtmlAgilityPack;


namespace Kalow.Apps.ApiTester.Parsers
{
    public class BaseParser
    {

        public HtmlNode GetNextSiblingByName(HtmlNode node, string name)
        {
            node = node.NextSibling;
            while (node != null)
            {
                if (node.OriginalName.Equals(name))
                {
                    break;
                }
                node = node.NextSibling;
            }
            return node;
        }
    }
}
