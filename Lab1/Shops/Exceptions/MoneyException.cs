namespace Shops.Exceptions;

public class MoneyException : Exception
{
    private MoneyException(decimal amount)
    {
        Amount = amount;
    }

    public decimal Amount { get; }

    public static MoneyException InvalidAmountOfMoney(decimal amount) => new MoneyException(amount);
}