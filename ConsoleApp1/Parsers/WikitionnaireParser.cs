using Crolow.FastDico.Models.Dictionary.Entities;
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
                var sectionNodes = languageNode.ParentNode.ParentNode.ChildNodes.Where(p => p.OriginalName == "section");

                foreach (var sectionNode in sectionNodes)
                {
                    if (sectionNode.ChildNodes.Any(p => p.HasClass("mw-heading3")))
                    {
                        if (sectionNode.DescendantNodes().Any(p => p.Id == "Étymologie"))
                        {
                            var node = sectionNode.ChildNodes.FirstOrDefault(p => p.OriginalName == "dl");
                            if (node != null)
                            {
                                var text = node.InnerText;
                                w.Etymology = HtmlEntity.DeEntitize(text);
                            }
                            continue;
                        }

                        var catgramNode = sectionNode.DescendantNodes().FirstOrDefault(p => p.HasClass("titredef"));
                        if (catgramNode != null)
                        {
                            currentDefinition = new DefinitionModel();
                            w.Definitions.Add(currentDefinition);
                            currentDefinition.CatGram = catgramNode.InnerText;
                            ParseDefinition(sectionNode, currentDefinition);
                        }
                        break;

                    }
                }
            }
            return w;
        }

        private void ParseDefinition(HtmlNode sectionNode, DefinitionModel currentDefinition)
        {
            var nameNode = sectionNode.ChildNodes.FirstOrDefault(p => p.OriginalName == "p");
            if (nameNode != null)
            {
                currentDefinition.Word = nameNode.ChildNodes.FirstOrDefault(p => p.Name.Equals("b"))?.InnerText ?? "";
                var forms = sectionNode.ChildNodes.Where(p => p.HasClass("ligne-de-forme"));
                if (forms.Any())
                {
                    currentDefinition.CatGram += " " + string.Join(" ", forms.Select(p => p.InnerText));
                }
            }

            var defnodes = sectionNode.ChildNodes.FirstOrDefault(p => p.OriginalName == "ol");
            if (defnodes != null)
            {
                foreach (var defNode in defnodes.ChildNodes.Where(p => p.OriginalName == "li"))
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
            }
        }
    }
}
