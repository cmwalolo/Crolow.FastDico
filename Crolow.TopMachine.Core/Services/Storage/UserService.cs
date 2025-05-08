using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data.Bridge;
using Crolow.TopMachine.Data.Bridge.Entities;

namespace Crolow.Pix.Core.Services.Storage
{
    public class UserService : IUserService
    {
        public IDataFactory dataFactory;

        public UserService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<IUser> LoadAll()
        {
            return dataFactory.Users.GetAllNodes().Result.ToList();
        }

        public void Update(IUser user)
        {
            dataFactory.Users.Update(user);
            user.EditState = EditState.Unchanged;
        }

    }
}
