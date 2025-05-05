namespace MauiBlazorWeb.Shared.Models
{
    public class MessageModel
    {
        public enum MessageType
        {
            Login = 0,
            Logout = 1,

            UpdateRoom = 12,
            GameStarted = 14,

            Change = 21,
            Pass = 22,
            Pick = 23,
            BoardRackSelected = 24,
            BoardRackSelectNext = 25,
            BoardRackSelectPrevious = 26,
            RoundIsPlayed = 27,
            SetNextPlayer = 28,
            PickOnly = 29,
            RobotPlay = 30,
        }

        public MessageType Type { get; set; }
        public bool MessageResult { get; set; }
        public object MessageObject { get; set; }
        public string Message { get; set; }

        public T GetMessage<T>()
        {
            return MessageObject != null ? (T)MessageObject : default(T);
        }
    }
}
