using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Models;

namespace Banks.Services;

public interface ICentralBank
{
    Bank AddBank(
        string name,
        decimal transferLimit,
        decimal withdrawalLimit,
        double balancePercentage,
        decimal minimalCreditBalance,
        decimal creditAccountCommission,
        DepositPercentage depositPercentage);

    Client AddClient(string name, string surname, Passport? passport, Address? address);

    DebitAccount AddDebitAccount(Bank bank, Client client);

    DepositAccount AddDepositAccount(Bank bank, Client client, decimal depositAmount, int depositDuration);

    CreditAccount AddCreditAccount(Bank bank, Client client, decimal creditBalance);

    void ExecuteTransaction(ITransaction transaction);

    void CancelTransaction(ITransaction transaction);

    void Update(int days);

    Bank GetBank(string name);

    Client GetClient(string name, string surname);

    IAccount GetAccount(string id);
}