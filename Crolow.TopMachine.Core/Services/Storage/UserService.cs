using Crolow.FastDico.Common.Interfaces;
using Crolow.FastDico.Common.Models.Common.Entities;
using Crolow.TopMachine.Core.Interfaces;
using Crolow.TopMachine.Data.Interfaces;

namespace Crolow.Pix.Core.Services.Storage
{
    public class UserService : IUserService
    {
        public IDataFactory dataFactory;

        public UserService(IDataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public List<User> LoadAll()
        {
            return dataFactory.Users.GetAllNodes().Result.ToList();
        }

        public void Update(User user)
        {
            dataFactory.Users.Update(user);
            user.EditState = EditState.Unchanged;
        }

    }
}
