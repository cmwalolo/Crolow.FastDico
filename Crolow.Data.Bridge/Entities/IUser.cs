namespace Crolow.TopMachine.Data.Bridge.Entities
{
    public interface IUser : IDataObject
    {
        string Name { get; set; }
        string Token { get; set; }
        string UserName { get; set; }
    }
}