using MauiBlazorWeb.Shared.Models;

namespace MauiBlazorWeb.Shared.Interfaces
{
    public interface IMessageService
    {
        event Action<MessageModel> OnMessage;
        event Action<HubMessageModel> OnHubMessage;

        void SendMessage(MessageModel message);
        void SendHubMessage(HubMessageModel message);
    }
}