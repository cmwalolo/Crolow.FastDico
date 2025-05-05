using MauiBlazorWeb.Shared.Interfaces;
using MauiBlazorWeb.Shared.Models;

namespace MauiBlazorWeb.Web.Services
{
    namespace BlazorApp.Services
    {

        public class MessageService : IMessageService
        {
            public event Action<MessageModel> OnMessage;
            public event Action<HubMessageModel> OnHubMessage;

            public void SendMessage(MessageModel message)
            {
                OnMessage?.Invoke(message);
            }

            public void SendHubMessage(HubMessageModel message)
            {
                OnHubMessage?.Invoke(message);
            }
        }
    }
}
