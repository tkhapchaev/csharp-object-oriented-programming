using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;
using Banks.Models;
using Banks.Services;
using Xunit;

namespace Banks.Test;

public class CentralBankTests
{
    private readonly CentralBank _centralBank;

    public CentralBankTests()
    {
        _centralBank = new CentralBank();
    }

    [Fact]
    public void TransferMoney_MoneyHasBeenSuccessfullyTransferred()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        Client client1 = _centralBank.AddClient(
            "Client 1",
            "Client 1",
            new Passport(100, 100),
            new Address("Russia", "Moscow", "Noviy arbat", 10));

        Client client2 = _centralBank.AddClient(
            "Client 2",
            "Client 2",
            new Passport(200, 200),
            new Address("Russia", "Moscow", "Noviy arbat", 15));

        DebitAccount debitAccount1 = _centralBank.AddDebitAccount(bank, client1);
        DebitAccount debitAccount2 = _centralBank.AddDebitAccount(bank, client2);

        var refill = new Refill(debitAccount1, 20000);
        var transfer = new Transfer(debitAccount1, debitAccount2, 10000);

        _centralBank.ExecuteTransaction(refill);
        _centralBank.ExecuteTransaction(transfer);

        Assert.Equal(10000, debitAccount1.GetBalance(), 1);
        Assert.Equal(10000, debitAccount2.GetBalance(), 1);
        Assert.Contains(transfer, _centralBank.Transactions);
    }

    [Fact]
    public void CheckReplenishment_ReplenishmentIsCorrect()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.65,
            500000,
            500,
            new DepositPercentage());

        Client client = _centralBank.AddClient(
            "Client",
            "Client",
            new Passport(200, 200),
            new Address("Russia", "Moscow", "Noviy arbat", 15));

        DebitAccount debitAccount = _centralBank.AddDebitAccount(bank, client);

        var refill = new Refill(debitAccount, 500000);
        _centralBank.ExecuteTransaction(refill);

        _centralBank.Update(31);

        Assert.Equal(650000, debitAccount.GetBalance());
    }

    [Fact]
    public void SetNewCreditAccountCommission_ClientHasReceivedNotification()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        Client client = _centralBank.AddClient(
            "Client",
            "Client",
            new Passport(200, 200),
            new Address("Russia", "Moscow", "Noviy arbat", 15));

        CreditAccount creditAccount = _centralBank.AddCreditAccount(bank, client, 55000);

        string expectedNotification =
            $"Notification from bank \"{bank.Name}\": the commission for using your account with a negative balance has been changed. New value: 800.";

        bank.SetCreditAccountCommission(800);

        Assert.Contains(expectedNotification, client.Notifications);
    }

    [Fact]
    public void CancelTransaction_TransactionHasBeenSuccessfullyCancelled()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        Client client = _centralBank.AddClient(
            "Client",
            "Client",
            new Passport(200, 200),
            new Address("Russia", "Moscow", "Noviy arbat", 15));

        DebitAccount debitAccount = _centralBank.AddDebitAccount(bank, client);

        var refill = new Refill(debitAccount, 500000);
        var withdrawal = new Withdrawal(debitAccount, 200000);

        _centralBank.ExecuteTransaction(refill);
        _centralBank.ExecuteTransaction(withdrawal);

        Assert.Contains(refill, _centralBank.Transactions);
        Assert.Contains(withdrawal, _centralBank.Transactions);

        _centralBank.CancelTransaction(withdrawal);

        Assert.Contains(refill, _centralBank.Transactions);
        Assert.DoesNotContain(withdrawal, _centralBank.Transactions);
    }

    [Fact]
    public void TransferMoneyBeforeDepositHasExpired_ThrowException()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        bank.AddDepositRange(new DepositRange(0, 2000000, 3));

        Client client1 = _centralBank.AddClient(
            "Client 1",
            "Client 1",
            new Passport(100, 100),
            new Address("Russia", "Moscow", "Noviy arbat", 10));

        Client client2 = _centralBank.AddClient(
            "Client 2",
            "Client 2",
            new Passport(200, 200),
            new Address("Russia", "Moscow", "Noviy arbat", 15));

        DepositAccount depositAccount = _centralBank.AddDepositAccount(bank, client1, 1000000, 50);
        DebitAccount debitAccount = _centralBank.AddDebitAccount(bank, client2);

        var refill = new Refill(depositAccount, 50000);
        var transfer = new Transfer(depositAccount, debitAccount, 10000);

        _centralBank.ExecuteTransaction(refill);

        Assert.Throws<AccountException>(() => _centralBank.ExecuteTransaction(transfer));
    }

    [Fact]
    public void WithdrawMoneyBeforeDepositHasExpired_ThrowException()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        bank.AddDepositRange(new DepositRange(0, 2000000, 3));

        Client client = _centralBank.AddClient(
            "Client",
            "Client",
            new Passport(100, 100),
            new Address("Russia", "Moscow", "Noviy arbat", 10));

        DepositAccount depositAccount = _centralBank.AddDepositAccount(bank, client, 1000000, 50);

        var refill = new Refill(depositAccount, 50000);
        var withdrawal = new Withdrawal(depositAccount, 10000);

        _centralBank.ExecuteTransaction(refill);

        Assert.Throws<AccountException>(() => _centralBank.ExecuteTransaction(withdrawal));
    }

    [Fact]
    public void ExceedWithdrawalLimitForRestrictedAccount_ThrowException()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        Client client = _centralBank.AddClient("Client", "Client", null, null);

        DebitAccount debitAccount = _centralBank.AddDebitAccount(bank, client);

        var refill = new Refill(debitAccount, 500000);
        var withdrawal = new Withdrawal(debitAccount, 200000);

        _centralBank.ExecuteTransaction(refill);

        Assert.Throws<AccountException>(() => _centralBank.ExecuteTransaction(withdrawal));
    }

    [Fact]
    public void ExceedTransferLimitForRestrictedAccount_ThrowException()
    {
        Bank bank = _centralBank.AddBank(
            "Bank",
            100000,
            100000,
            3.5,
            500000,
            500,
            new DepositPercentage());

        Client client1 = _centralBank.AddClient("Client 1", "Client 1", null, null);

        Client client2 = _centralBank.AddClient(
            "Client 2",
            "Client 2",
            new Passport(100, 100),
            new Address("Russia", "Moscow", "Noviy arbat", 10));

        DebitAccount debitAccount1 = _centralBank.AddDebitAccount(bank, client1);
        DebitAccount debitAccount2 = _centralBank.AddDebitAccount(bank, client2);

        var refill = new Refill(debitAccount1, 500000);
        var transfer = new Transfer(debitAccount1, debitAccount2, 200000);

        _centralBank.ExecuteTransaction(refill);

        Assert.Throws<AccountException>(() => _centralBank.ExecuteTransaction(transfer));
    }
}