using Banks.Entities.Clients;

namespace Banks.Entities.Publisher;

public class Subscribers
{
    private readonly List<Client> _clients;

    public Subscribers()
    {
        _clients = new List<Client>();
    }

    public IReadOnlyCollection<Client> Clients => _clients.AsReadOnly();

    public void AddClient(Client client)
    {
        _clients.Add(client);
    }

    public void RemoveClient(Client client)
    {
        _clients.Remove(client);
    }
}