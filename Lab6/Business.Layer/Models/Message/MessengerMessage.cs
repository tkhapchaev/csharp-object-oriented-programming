namespace Business.Layer.Models.Message;

public class MessengerMessage : IMessage
{
    public MessengerMessage(string contents, DateTime time, string source)
    {
        Id = Guid.NewGuid();
        Contents = contents ?? throw new ArgumentNullException(nameof(contents));
        Source = source ?? throw new ArgumentNullException(nameof(source));
        MessageType = "Messenger";
        Time = time;
        MessageStatus = MessageStatus.New;
    }

    public Guid Id { get; }

    public string Contents { get; }

    public string MessageType { get; }

    public DateTime Time { get; }

    public string Source { get; }

    public MessageStatus MessageStatus { get; private set; }

    public void MakeReceived()
    {
        MessageStatus = MessageStatus.Received;
    }

    public void MakeProcessed()
    {
        MessageStatus = MessageStatus.Processed;
    }

    public override string ToString()
    {
        return $"ID: {Id}; [{Time}] \"{Contents}\" from {Source} ({MessageType}), status: [{MessageStatus}].";
    }
}