namespace Banks.Exceptions;

public class DepositPercentageException : Exception
{
    private DepositPercentageException(string message)
        : base(message)
    {
    }

    public static DepositPercentageException NoAppropriateRange(decimal money) =>
        new DepositPercentageException($"There is no appropriate deposit range for such amount of money: \"{money}\".");
}