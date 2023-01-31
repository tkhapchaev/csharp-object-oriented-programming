using System.Globalization;
using Banks.Entities.Accounts;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Models;
using Banks.Services;

namespace Banks.Console;

public class BanksConsoleHandler
{
    private readonly CentralBank _centralBank;

    public BanksConsoleHandler()
    {
        _centralBank = new CentralBank();
    }

    public void AddBank(
        string name,
        string transferLimit,
        string withdrawalLimit,
        string balancePercentage,
        string minimalCreditBalance,
        string creditAccountCommission)
    {
        var depositPercentage = new DepositPercentage();
        depositPercentage.AddDepositRange(new DepositRange(0, 50000, 3));
        depositPercentage.AddDepositRange(new DepositRange(50000, 100000, 3.5));
        depositPercentage.AddDepositRange(new DepositRange(100000, int.MaxValue, 4));

        _centralBank.AddBank(
            name,
            Convert.ToDecimal(transferLimit),
            Convert.ToDecimal(withdrawalLimit),
            Convert.ToDouble(balancePercentage, CultureInfo.InvariantCulture),
            Convert.ToDecimal(minimalCreditBalance),
            Convert.ToDecimal(creditAccountCommission),
            depositPercentage);
    }

    public void AddClient(
        string name,
        string surname,
        string passportSeries,
        string passportNumber,
        string country,
        string city,
        string street,
        string houseNumber)
    {
        _centralBank.AddClient(
            name,
            surname,
            new Passport(Convert.ToInt32(passportSeries), Convert.ToInt32(passportNumber)),
            new Address(country, city, street, Convert.ToInt32(houseNumber)));
    }

    public string AddDebitAccount(string bankName, string clientName, string clientSurname)
    {
        DebitAccount debitAccount = _centralBank.AddDebitAccount(
            _centralBank.GetBank(bankName),
            _centralBank.GetClient(clientName, clientSurname));

        return debitAccount.Id.ToString();
    }

    public string AddDepositAccount(
        string bankName,
        string clientName,
        string clientSurname,
        string depositAmount,
        string depositDuration)
    {
        DepositAccount depositAccount = _centralBank.AddDepositAccount(
            _centralBank.GetBank(bankName),
            _centralBank.GetClient(clientName, clientSurname),
            Convert.ToDecimal(depositAmount),
            Convert.ToInt32(depositDuration));

        return depositAccount.Id.ToString();
    }

    public string AddCreditAccount(string bankName, string clientName, string clientSurname, string creditBalance)
    {
        CreditAccount creditAccount = _centralBank.AddCreditAccount(
            _centralBank.GetBank(bankName),
            _centralBank.GetClient(clientName, clientSurname),
            Convert.ToDecimal(creditBalance));

        return creditAccount.Id.ToString();
    }

    public void TopUp(string accountId, string money)
    {
        var refill = new Refill(_centralBank.GetAccount(accountId), Convert.ToDecimal(money));

        _centralBank.ExecuteTransaction(refill);
    }

    public void Withdraw(string accountId, string money)
    {
        var withdrawal = new Withdrawal(_centralBank.GetAccount(accountId), Convert.ToDecimal(money));

        _centralBank.ExecuteTransaction(withdrawal);
    }

    public void Transfer(string accountFromId, string accountToId, string money)
    {
        var transfer = new Transfer(
            _centralBank.GetAccount(accountFromId),
            _centralBank.GetAccount(accountToId),
            Convert.ToDecimal(money));

        _centralBank.ExecuteTransaction(transfer);
    }

    public string GetBalance(string accountId)
    {
        return _centralBank.GetAccount(accountId).GetBalance().ToString(CultureInfo.InvariantCulture);
    }

    public List<string> GetAccounts(string name, string surname)
    {
        Client client = _centralBank.GetClient(name, surname);
        var result = client.Accounts.Select(account => $"{account.ToString()} with ID {account.Id}.").ToList();

        return result;
    }
}