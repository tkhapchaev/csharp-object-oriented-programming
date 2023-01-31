using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions;

public class Transfer : ITransaction
{
    private readonly IAccount _accountFrom;

    private readonly IAccount _accountTo;

    private readonly decimal _money;

    public Transfer(IAccount accountFrom, IAccount accountTo, decimal money)
    {
        _accountFrom = accountFrom ?? throw new ArgumentNullException(nameof(accountFrom));
        _accountTo = accountTo ?? throw new ArgumentNullException(nameof(accountTo));
        _money = money;
    }

    public void Execute()
    {
        _accountFrom.Transfer(_accountTo, _money);
    }

    public void Cancel()
    {
        _accountTo.Transfer(_accountFrom, _money);
    }
}