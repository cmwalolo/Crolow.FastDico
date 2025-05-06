using Crolow.FastDico.Models.Common.Entities;

namespace Crolow.TopMachine.Core.Interfaces
{
    public interface IUserService
    {
        List<User> LoadAll();
        void Update(User user);
    }
}