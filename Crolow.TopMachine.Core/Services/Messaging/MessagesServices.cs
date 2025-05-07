using Crolow.FastDico.Common.Interfaces.Messaging;
using Crolow.FastDico.Common.Models.Common.Messaging;

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
