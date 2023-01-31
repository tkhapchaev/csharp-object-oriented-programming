namespace Banks.Exceptions;

public class AccountException : Exception
{
    private AccountException(string message)
        : base(message)
    {
    }

    public static AccountException InvalidBalance(decimal money) =>
        new AccountException($"Invalid account balance: \"{money}\".");

    public static AccountException InvalidDepositDuration(int depositDuration) =>
        new AccountException($"Invalid deposit duration: \"{depositDuration}\".");

    public static AccountException WithdrawalIsNotAllowed(decimal money) =>
        new AccountException($"Withdrawal ({money}) is not allowed for this account.");

    public static AccountException TransferIsNotAllowed(decimal money) =>
        new AccountException($"Transfer ({money}) is not allowed for this account.");
}