namespace Banks.Exceptions;

public class BankException : Exception
{
    private BankException(string message)
        : base(message)
    {
    }

    public static BankException InvalidLimitValue(decimal money) =>
        new BankException($"Invalid limit value: \"{money}\".");

    public static BankException InvalidPercentageValue(double money) =>
        new BankException($"Invalid percentage value: \"{money}\".");

    public static BankException InvalidCommissionValue(decimal money) =>
        new BankException($"Invalid commission value: \"{money}\".");

    public static BankException InvalidCreditBalanceValue(decimal money) =>
        new BankException($"Invalid credit balance value: \"{money}\".");
}