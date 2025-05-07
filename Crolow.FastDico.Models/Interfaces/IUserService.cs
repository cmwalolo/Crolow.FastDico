using Crolow.FastDico.Common.Models.Common.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface IUserService
    {
        List<User> LoadAll();
        void Update(User user);
    }
}