using Banks.Entities.Accounts;
using Banks.Entities.Clients;
using Banks.Entities.Publisher;
using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities.Banks;

public class Bank : IBank, IPublisher
{
    private readonly List<Client> _clients;

    private readonly Dictionary<string, Subscribers> _subscribers;

    private readonly DepositPercentage _depositPercentage;

    private decimal _transferLimit;

    private decimal _withdrawalLimit;

    private double _balancePercentage;

    private decimal _minimalCreditBalance;

    private decimal _creditAccountCommission;

    public Bank(
        string name,
        decimal transferLimit,
        decimal withdrawalLimit,
        double balancePercentage,
        decimal minimalCreditBalance,
        decimal creditAccountCommission,
        DepositPercentage depositPercentage)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _depositPercentage = depositPercentage ?? throw new ArgumentNullException(nameof(depositPercentage));

        _transferLimit = transferLimit;
        _withdrawalLimit = withdrawalLimit;
        _balancePercentage = balancePercentage;
        _minimalCreditBalance = minimalCreditBalance;
        _creditAccountCommission = creditAccountCommission;

        _clients = new List<Client>();
        _subscribers = new Dictionary<string, Subscribers>
        {
            ["Debit account"] = new Subscribers(),
            ["Deposit account"] = new Subscribers(),
            ["Credit account"] = new Subscribers(),
        };
    }

    public string Name { get; }

    public decimal GetTransferLimit() => _transferLimit;

    public decimal GetWithdrawalLimit() => _withdrawalLimit;

    public double GetBalancePercentage() => _balancePercentage;

    public decimal GetMinimalCreditBalance() => _minimalCreditBalance;

    public decimal GetCreditAccountCommission() => _creditAccountCommission;

    public DepositPercentage GetDepositPercentage() => _depositPercentage;

    public IReadOnlyCollection<Client> Clients() => _clients.AsReadOnly();

    public void SetTransferLimit(decimal value)
    {
        if (value <= 0)
        {
            throw BankException.InvalidLimitValue(value);
        }

        _transferLimit = value;

        string notification =
            $"Notification from bank \"{Name}\". New transfer limit for restricted accounts: {value}.";

        Notify("Debit account", notification);
        Notify("Deposit account", notification);
        Notify("Credit account", notification);
    }

    public void SetWithdrawalLimit(decimal value)
    {
        if (value <= 0)
        {
            throw BankException.InvalidLimitValue(value);
        }

        _withdrawalLimit = value;

        string notification =
            $"Notification from bank \"{Name}\". New withdrawal limit for restricted accounts: {value}.";

        Notify("Debit account", notification);
        Notify("Deposit account", notification);
        Notify("Credit account", notification);
    }

    public void SetBalancePercentage(double value)
    {
        if (value <= 0)
        {
            throw BankException.InvalidPercentageValue(value);
        }

        _balancePercentage = value;

        string notification =
            $"Notification from bank \"{Name}\": your percentage on the balance has been changed. New value: {value}.";

        Notify("Debit account", notification);
        Notify("Deposit account", notification);
    }

    public void SetMinimalCreditBalance(decimal value)
    {
        if (value <= 0)
        {
            throw BankException.InvalidLimitValue(value);
        }

        _minimalCreditBalance = value;

        string notification =
            $"Notification from bank \"{Name}\": minimal credit balance has been changed. New value: {value}.";

        Notify("Credit account", notification);
    }

    public void SetCreditAccountCommission(decimal value)
    {
        if (value <= 0)
        {
            throw BankException.InvalidLimitValue(value);
        }

        _creditAccountCommission = value;

        string notification =
            $"Notification from bank \"{Name}\": the commission for using your account with a negative balance has been changed. New value: {value}.";

        Notify("Credit account", notification);
    }

    public void AddDepositRange(DepositRange depositRange)
    {
        _depositPercentage.AddDepositRange(depositRange);

        string notification =
            $"Notification from bank \"{Name}\": new rate for a deposit from {depositRange.RangeStart} to {depositRange.RangeEnd} is {depositRange.Percent}.";

        Notify("Deposit account", notification);
    }

    public void RemoveDepositRange(DepositRange depositRange)
    {
        _depositPercentage.RemoveDepositRange(depositRange);
    }

    public double GetDepositPercentage(decimal money)
    {
        return _depositPercentage.GetDepositPercentage(money);
    }

    public void AddClient(Client client)
    {
        _clients.Add(client);

        foreach (IAccount account in client.Accounts)
        {
            Subscribe(account.ToString() !, client);
        }
    }

    public void RemoveClient(Client client)
    {
        _clients.Remove(client);

        foreach (IAccount account in client.Accounts)
        {
            Unsubscribe(account.ToString() !, client);
        }
    }

    public void Subscribe(string eventType, Client client)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        _subscribers[eventType].AddClient(client);
    }

    public void Unsubscribe(string eventType, Client client)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        _subscribers[eventType].RemoveClient(client);
    }

    private void Notify(string eventType, string data)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        foreach (Client client in _subscribers[eventType].Clients)
        {
            client.Update(data);
        }
    }
}