namespace Banks.Entities.Transactions;

public interface ITransaction
{
    void Execute();

    void Cancel();
}