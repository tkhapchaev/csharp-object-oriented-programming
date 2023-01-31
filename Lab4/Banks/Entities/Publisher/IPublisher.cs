using Banks.Entities.Clients;

namespace Banks.Entities.Publisher;

public interface IPublisher
{
    void Subscribe(string eventType, Client client);

    void Unsubscribe(string eventType, Client client);
}