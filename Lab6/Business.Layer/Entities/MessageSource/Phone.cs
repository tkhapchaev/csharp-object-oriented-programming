using Business.Layer.Models.Message;

namespace Business.Layer.Entities.MessageSource;

public class Phone : IMessageSource
{
    public Phone(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }

    public IMessage NewMessage(string contents)
    {
        return new SmsMessage(contents, DateTime.Now, Name);
    }
}