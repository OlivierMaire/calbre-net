namespace calibre_net.Client.Services;

public interface IMessageService
{
    event Action<MessageEventArgs> OnMessage;
    void SendMessage(MessageEventArgs message);
    void ClearMessages();

    void Subscribe(Action<MessageEventArgs> handler, Guid? windowId);
    void Subscribe(Action<MessageEventArgs> handler, Guid? windowId, MessageType eventType);
}


public class MessageEventArgs
{
    public MessageType EventType { get; set; }

    public object Payload {get;set;}

    public Type PayloadType {get;set;}
    public string Message { get; set; }

    public Guid WindowId { get; set; }
}

   public enum MessageType {
        DrawerToggle,
        Message
    }
