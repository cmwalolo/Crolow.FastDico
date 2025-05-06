using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Models.Common.Entities;

public class User : DataObject
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; }
}
