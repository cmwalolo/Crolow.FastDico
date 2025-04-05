using Crolow.FastDico.Models.Models.Dictionary.Entities;
using HtmlAgilityPack;
using static Kalow.Apps.ApiTester.DictionaryCrawler;

namespace Kalow.Apps.ApiTester.Parsers
{
    public class WikitionnaireParser : BaseParser
    {
        public WordEntryModel Parse(string word, string input, AvailableSite site)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);

            WordEntryModel w = new WordEntryModel();
            w.Word = word;
            w.NormalizedWord = word.NormalizeString();
            w.Source = "wikitionnaire";

            var currentDefinition = new DefinitionModel();

            var languageNode = htmlDoc.DocumentNode.DescendantNodes().Where(p => p.Id == "Français").FirstOrDefault();

            if (languageNode != null)
            {
                var currentNode = languageNode.ParentNode.NextSibling;
                var navigate = true;
                var searchType = 0;
                while (navigate && currentNode != null)
                {
                    if (currentNode.HasClass("mw-heading2"))
                    {
                        break;
                    }
                    if (currentNode.HasClass("mw-heading3"))
                    {
                        // Is there Etymology or CatGram
                        switch (searchType)
                        {
                            case int i when i < 2:
                                if (currentNode.ChildNodes.Any(p => p.Id == "Étymologie"))
                                {

                                    var etymologyNode = GetNextSiblingByName(currentNode, "dl");
                                    if (etymologyNode != null)
                                    {
                                        var text = etymologyNode.InnerText;
                                        w.Etymology = HtmlEntity.DeEntitize(text);
                                    }

                                    searchType = 1;
                                    break;
                                }

                                var catgramNode = currentNode.DescendantNodes().FirstOrDefault(p => p.HasClass("titredef"));
                                if (catgramNode != null)
                                {
                                    currentDefinition = new DefinitionModel();
                                    w.Definitions.Add(currentDefinition);
                                    currentDefinition.CatGram = catgramNode.InnerText;
                                    searchType = 2;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (searchType)
                        {
                            case 2:
                                // We extend the catgram
                                if (currentNode.OriginalName.Equals("p"))
                                {
                                    currentDefinition.Word = currentNode.ChildNodes.FirstOrDefault(p => p.Name.Equals("b"))?.InnerText ?? "";
                                    var forms = currentNode.ChildNodes.Where(p => p.HasClass("ligne-de-forme"));
                                    if (forms.Any())
                                    {
                                        currentDefinition.CatGram += " " + string.Join(" ", forms.Select(p => p.InnerText));
                                    }
                                    searchType = 3;
                                }
                                break;
                            case 3:
                                // We scan definitions
                                if (currentNode.OriginalName.Equals("ol"))
                                {
                                    foreach (var defNode in currentNode.ChildNodes.Where(p => p.Name == "li"))
                                    {
                                        var exampleNode = defNode.ChildNodes.FirstOrDefault(p => p.OriginalName == "ul");
                                        if (exampleNode != null)
                                        {
                                            exampleNode.Remove();
                                        }

                                        var tagNodes = defNode.ChildNodes;
                                        foreach (var tagnode in tagNodes.ToList())
                                        {
                                            if (tagnode.OriginalName.Equals("span") || tagnode.OriginalName.Equals("i"))
                                            {
                                                string text = tagnode.InnerText.Replace("(", "").Replace(")", "");

                                                if (tagnode.HasClass("emploi"))
                                                {
                                                    if (!currentDefinition.Usages.Any(p => p == text))
                                                    {
                                                        currentDefinition.Usages.Add(HtmlEntity.DeEntitize(text));
                                                    }
                                                }
                                                else
                                                {
                                                    if (!currentDefinition.Domains.Any(p => p == text))
                                                    {
                                                        currentDefinition.Domains.Add(HtmlEntity.DeEntitize(text));
                                                    }
                                                }
                                                // tagnode.Remove();
                                            }
                                            else
                                            {
                                                if (tagnode.InnerText != " ")
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        var def = defNode.InnerText.Replace("\n", "").Trim();
                                        currentDefinition.Definitions.Add(HtmlEntity.DeEntitize(def));
                                    }

                                    searchType = 0;
                                }
                                break;
                        }
                    }
                    currentNode = currentNode.NextSibling;
                }
            }
            return w;
        }

    }
}
