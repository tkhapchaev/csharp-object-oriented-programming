using Banks.Entities.Accounts;
using Banks.Entities.Publisher;
using Banks.Models;

namespace Banks.Entities.Clients;

public class Client : ISubscriber
{
    private readonly List<IAccount> _accounts;

    private readonly List<string> _notifications;

    public Client(string name, string surname)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Surname = surname ?? throw new ArgumentNullException(nameof(surname));

        _accounts = new List<IAccount>();
        _notifications = new List<string>();
    }

    public IReadOnlyCollection<IAccount> Accounts => _accounts.AsReadOnly();

    public IReadOnlyCollection<string> Notifications => _notifications.AsReadOnly();

    public string Name { get; }

    public string Surname { get; }

    public Passport? Passport { get; set; }

    public Address? Address { get; set; }

    public void AddAccount(IAccount account)
    {
        _accounts.Add(account);
    }

    public void RemoveAccount(IAccount account)
    {
        _accounts.Remove(account);
    }

    public void Update(string data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _notifications.Add(data);
    }
}