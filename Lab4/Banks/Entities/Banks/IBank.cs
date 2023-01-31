using Banks.Entities.Accounts;
using Banks.Models;

namespace Banks.Entities.Banks;

public interface IBank
{
    string Name { get; }

    decimal GetTransferLimit();

    decimal GetWithdrawalLimit();

    double GetBalancePercentage();

    decimal GetMinimalCreditBalance();

    decimal GetCreditAccountCommission();

    double GetDepositPercentage(decimal money);

    DepositPercentage GetDepositPercentage();

    void SetTransferLimit(decimal value);

    void SetWithdrawalLimit(decimal value);

    void SetBalancePercentage(double value);

    void SetMinimalCreditBalance(decimal value);

    void SetCreditAccountCommission(decimal value);

    void AddDepositRange(DepositRange depositRange);

    void RemoveDepositRange(DepositRange depositRange);
}