using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions;

public class Refill : ITransaction
{
    private readonly IAccount _account;

    private readonly decimal _money;

    public Refill(IAccount account, decimal money)
    {
        _account = account ?? throw new ArgumentNullException(nameof(account));
        _money = money;
    }

    public void Execute()
    {
        _account.TopUp(_money);
    }

    public void Cancel()
    {
        _account.Withdraw(_money);
    }
}