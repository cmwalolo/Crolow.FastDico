namespace Crolow.TopMachine.Data.Bridge.Entities.Definitions
{
    public interface IDictionaryModel : IDataObject
    {
        string DictionaryFile { get; set; }
        List<IDictionaryLookup> DictionaryUrls { get; set; }
        bool IsDefault { get; set; }
        string Language { get; set; }
        string Name { get; set; }
    }
}