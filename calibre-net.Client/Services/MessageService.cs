namespace calibre_net.Client.Services;


[SingletonRegistration]
public class MessageService : IMessageService
{
    public event Action<MessageEventArgs>? OnMessage;
    public void SendMessage(MessageEventArgs message)
    {
        Console.WriteLine($"Sending Message : {message.EventType}, GUID: {message.WindowId}");
        OnMessage?.Invoke(message);
    }

    public void ClearMessages()
    {
        // OnMessage?.Invoke(new MessageEventArgs());
    }

    public void Subscribe(Action<MessageEventArgs> handler, Guid? windowId = null)
    {
        OnMessage += (e) =>
        {
            if (windowId == null || e.WindowId == windowId)
                handler(e);
        };
    }

    public void Subscribe(Action<MessageEventArgs> handler, Guid? windowId, MessageType eventType)

    {
        OnMessage += (e) =>
          {
              if (windowId == null || e.WindowId == windowId)
              {
                  if (e.EventType == eventType)
                      handler(e);
              }
          };
    }


}
