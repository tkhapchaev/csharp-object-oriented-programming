using Business.Layer.Models.Message;

namespace Business.Layer.Entities.MessageSource;

public interface IMessageSource
{
    string Name { get; }

    IMessage NewMessage(string contents);
}