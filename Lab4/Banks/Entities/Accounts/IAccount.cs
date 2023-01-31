using Banks.Entities.Banks;
using Banks.Entities.Clients;

namespace Banks.Entities.Accounts;

public interface IAccount
{
    Bank Bank { get; }

    Client Owner { get; }

    Guid Id { get; }

    decimal GetBalance();

    bool IsRestricted();

    void TopUp(decimal money);

    void Withdraw(decimal money);

    void Transfer(IAccount account, decimal money);

    void Update();
}