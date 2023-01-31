namespace Banks.Exceptions;

public class MoneyException : Exception
{
    private MoneyException(string message)
        : base(message)
    {
    }

    public static MoneyException InvalidAmountOfMoney(decimal money) =>
        new MoneyException($"Invalid amount of money: \"{money}\".");
}