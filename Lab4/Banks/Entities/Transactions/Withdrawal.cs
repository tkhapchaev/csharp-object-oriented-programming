using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions;

public class Withdrawal : ITransaction
{
    private readonly IAccount _account;

    private readonly decimal _money;

    public Withdrawal(IAccount account, decimal money)
    {
        _account = account ?? throw new ArgumentNullException(nameof(account));
        _money = money;
    }

    public void Execute()
    {
        _account.Withdraw(_money);
    }

    public void Cancel()
    {
        _account.TopUp(_money);
    }
}