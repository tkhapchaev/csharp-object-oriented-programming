namespace Banks.Exceptions;

public class CentralBankException : Exception
{
    private CentralBankException(string message)
        : base(message)
    {
    }

    public static CentralBankException ThereWasNoSuchTransaction() =>
        new CentralBankException($"Unable to cancel transaction: there was no such transaction.");

    public static CentralBankException NoSuchBank(string name) =>
        new CentralBankException($"Unable to find bank with the name \"{name}\".");

    public static CentralBankException NoSuchClient(string name, string surname) =>
        new CentralBankException($"Unable to find client with the name \"{name}\" and surname \"{surname}\".");

    public static CentralBankException NoSuchAccount(string id) =>
        new CentralBankException($"Unable to find account with the id \"{id}\".");
}