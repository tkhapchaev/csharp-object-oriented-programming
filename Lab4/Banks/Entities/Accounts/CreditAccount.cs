using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Exceptions;

namespace Banks.Entities.Accounts;

public class CreditAccount : IAccount
{
    private decimal _balance;

    private bool _isRestricted;

    public CreditAccount(Bank bank, Client owner, DateTime creationDate, decimal balance)
    {
        Bank = bank ?? throw new ArgumentNullException(nameof(bank));
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        CreationDate = creationDate;
        Id = Guid.NewGuid();

        if (balance < Bank.GetMinimalCreditBalance())
        {
            AccountException.InvalidBalance(balance);
        }

        _balance = balance;

        if (Owner.Address is null && Owner.Passport is null)
        {
            _isRestricted = true;
        }
        else
        {
            _isRestricted = false;
        }
    }

    public Bank Bank { get; }

    public Client Owner { get; }

    public DateTime CreationDate { get; }

    public Guid Id { get; }

    public decimal GetBalance() => _balance;

    public bool IsRestricted() => _isRestricted;

    public void TopUp(decimal money)
    {
        if (money <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(money);
        }

        _balance += money;
    }

    public void Withdraw(decimal money)
    {
        if (money <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(money);
        }

        if (_isRestricted && money > Bank.GetWithdrawalLimit())
        {
            throw AccountException.WithdrawalIsNotAllowed(money);
        }

        _balance -= money;
    }

    public void Transfer(IAccount account, decimal money)
    {
        if (money <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(money);
        }

        if (_isRestricted && money > Bank.GetTransferLimit())
        {
            throw AccountException.TransferIsNotAllowed(money);
        }

        Withdraw(money);
        account.TopUp(money);
    }

    public void Update()
    {
        if (Owner.Address is not null || Owner.Passport is not null)
        {
            _isRestricted = false;
        }

        if (_balance < 0)
        {
            Withdraw(Bank.GetCreditAccountCommission());
        }
    }

    public override string ToString()
    {
        return "Credit account";
    }
}