using Crolow.TopMachine.Data.Bridge.Entities.Definitions;

namespace Crolow.FastDico.Common.Models.Dictionary.Entities
{
    public class DefinitionModel : IDefinitionModel
    {
        public string Word { get; set; }
        public string CatGram { get; set; } = string.Empty;
        public List<string> Definitions { get; set; } = new List<string>();
        public List<string> Domains { get; set; } = new List<string>();
        public List<string> Usages { get; set; } = new List<string>();
    }
}
