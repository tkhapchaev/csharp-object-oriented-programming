using Business.Layer.Models.Message;

namespace Business.Layer.Entities.MessageSource;

public class Messenger : IMessageSource
{
    public Messenger(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }

    public IMessage NewMessage(string contents)
    {
        return new MessengerMessage(contents, DateTime.Now, Name);
    }
}