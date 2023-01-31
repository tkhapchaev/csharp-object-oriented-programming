namespace Business.Layer.Models.Message;

public interface IMessage
{
    Guid Id { get; }

    string Contents { get; }

    string MessageType { get; }

    DateTime Time { get; }

    string Source { get; }

    MessageStatus MessageStatus { get; }

    void MakeReceived();

    void MakeProcessed();

    string ToString();
}