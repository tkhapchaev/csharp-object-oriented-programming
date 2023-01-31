using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Exceptions;

namespace Banks.Entities.Accounts;

public class DepositAccount : IAccount
{
    private decimal _balance;

    private bool _isRestricted;

    public DepositAccount(Bank bank, Client owner, DateTime creationDate, decimal depositAmount, int depositDuration)
    {
        Bank = bank ?? throw new ArgumentNullException(nameof(bank));
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        CreationDate = creationDate;
        DepositPercentage = Bank.GetDepositPercentage(depositAmount);
        DaysBeforeReplenishment = 30;
        Replenishment = 0;
        _balance = depositAmount;
        Id = Guid.NewGuid();

        if (depositDuration < 30)
        {
            throw AccountException.InvalidDepositDuration(depositDuration);
        }

        DepositDuration = depositDuration;

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

    public int DaysBeforeReplenishment { get; private set; }

    public int DepositDuration { get; private set; }

    public decimal Replenishment { get; private set; }

    public double DepositPercentage { get; }

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

        if (_balance - money < 0)
        {
            throw AccountException.WithdrawalIsNotAllowed(money);
        }

        if (_isRestricted && money > Bank.GetWithdrawalLimit())
        {
            throw AccountException.WithdrawalIsNotAllowed(money);
        }

        if (DepositDuration != 0)
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

        if (_balance - money < 0)
        {
            throw AccountException.TransferIsNotAllowed(money);
        }

        if (_isRestricted && money > Bank.GetTransferLimit())
        {
            throw AccountException.TransferIsNotAllowed(money);
        }

        if (DepositDuration != 0)
        {
            throw AccountException.TransferIsNotAllowed(money);
        }

        Withdraw(money);
        account.TopUp(money);
    }

    public void Update()
    {
        const int numberOfDaysInYear = 365;

        if (Owner.Address is not null || Owner.Passport is not null)
        {
            _isRestricted = false;
        }

        DaysBeforeReplenishment -= 1;
        Replenishment += _balance * (decimal)(DepositPercentage / numberOfDaysInYear);

        if (DaysBeforeReplenishment == 0)
        {
            DaysBeforeReplenishment = 30;
            _balance += Replenishment;
            Replenishment = 0;
        }

        if (DepositDuration != 0)
        {
            DepositDuration -= 1;
        }
    }

    public override string ToString()
    {
        return "Deposit account";
    }
}