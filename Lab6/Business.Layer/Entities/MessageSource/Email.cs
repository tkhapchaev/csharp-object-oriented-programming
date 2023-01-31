using Business.Layer.Models.Message;

namespace Business.Layer.Entities.MessageSource;

public class Email : IMessageSource
{
    public Email(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }

    public IMessage NewMessage(string contents)
    {
        return new EmailMessage(contents, DateTime.Now, Name);
    }
}