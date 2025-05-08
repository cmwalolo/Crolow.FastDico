using Crolow.TopMachine.Data.Bridge.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface IUserService
    {
        List<IUser> LoadAll();
        void Update(IUser user);
    }
}