using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;
using Banks.Models;

namespace Banks.Services;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks;

    private readonly List<Client> _clients;

    private readonly List<IAccount> _accounts;

    private readonly List<ITransaction> _transactions;

    private DateTime _currentDate;

    public CentralBank()
    {
        _currentDate = new DateTime(2000, 1, 3);
        _banks = new List<Bank>();
        _clients = new List<Client>();
        _accounts = new List<IAccount>();
        _transactions = new List<ITransaction>();
    }

    public IReadOnlyList<ITransaction> Transactions => _transactions.AsReadOnly();

    public IReadOnlyList<IBank> Banks => _banks.AsReadOnly();

    public Bank AddBank(
        string name,
        decimal transferLimit,
        decimal withdrawalLimit,
        double balancePercentage,
        decimal minimalCreditBalance,
        decimal creditAccountCommission,
        DepositPercentage depositPercentage)
    {
        var bank = new Bank(
            name,
            transferLimit,
            withdrawalLimit,
            balancePercentage,
            minimalCreditBalance,
            creditAccountCommission,
            depositPercentage);

        _banks.Add(bank);

        return bank;
    }

    public Client AddClient(string name, string surname, Passport? passport, Address? address)
    {
        var clientBuilder = new ClientBuilder();

        Client client = clientBuilder
            .WithName(name)
            .WithSurname(surname)
            .WithPassport(passport)
            .WithAddress(address)
            .Build();

        _clients.Add(client);

        return client;
    }

    public DebitAccount AddDebitAccount(Bank bank, Client client)
    {
        var debitAccount = new DebitAccount(bank, client, _currentDate);

        client.AddAccount(debitAccount);
        bank.AddClient(client);

        _accounts.Add(debitAccount);

        return debitAccount;
    }

    public DepositAccount AddDepositAccount(Bank bank, Client client, decimal depositAmount, int depositDuration)
    {
        var depositAccount = new DepositAccount(bank, client, _currentDate, depositAmount, depositDuration);

        client.AddAccount(depositAccount);
        bank.AddClient(client);

        _accounts.Add(depositAccount);

        return depositAccount;
    }

    public CreditAccount AddCreditAccount(Bank bank, Client client, decimal creditBalance)
    {
        var creditAccount = new CreditAccount(bank, client, _currentDate, creditBalance);

        client.AddAccount(creditAccount);
        bank.AddClient(client);

        _accounts.Add(creditAccount);

        return creditAccount;
    }

    public void ExecuteTransaction(ITransaction transaction)
    {
        transaction.Execute();
        _transactions.Add(transaction);
    }

    public void CancelTransaction(ITransaction transaction)
    {
        if (!_transactions.Contains(transaction))
        {
            throw CentralBankException.ThereWasNoSuchTransaction();
        }

        transaction.Cancel();
        _transactions.Remove(transaction);
    }

    public void Update(int days)
    {
        foreach (IAccount account in _accounts)
        {
            for (int i = 0; i < days; i++)
            {
                account.Update();
            }
        }

        _currentDate = _currentDate.AddDays(days);
    }

    public Bank GetBank(string name)
    {
        Bank? bank = _banks.FirstOrDefault(bank => bank.Name == name);

        if (bank is null)
        {
            throw CentralBankException.NoSuchBank(name);
        }

        return bank;
    }

    public Client GetClient(string name, string surname)
    {
        Client? client = _clients.FirstOrDefault(client => client.Name == name && client.Surname == surname);

        if (client is null)
        {
            throw CentralBankException.NoSuchClient(name, surname);
        }

        return client;
    }

    public IAccount GetAccount(string id)
    {
        IAccount? account = _accounts.FirstOrDefault(account => account.Id.ToString() == id);

        if (account is null)
        {
            throw CentralBankException.NoSuchAccount(id);
        }

        return account;
    }
}