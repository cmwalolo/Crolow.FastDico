namespace MauiBlazorWeb.Shared.Models
{
    public class UserGroupMessage
    {
        public string GroupName { get; set; }
        public string UserName { get; set; }

        public UserGroupMessage(string group, string user)
        {
            GroupName = group;
            UserName = user;
        }
    }
}
