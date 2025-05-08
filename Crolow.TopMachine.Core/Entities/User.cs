using Crolow.TopMachine.Data.Bridge.Entities;
using Kalow.Apps.Models.Data;

namespace Crolow.FastDico.Common.Models.Common.Entities;

public class User : DataObject, IUser
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; }
}
