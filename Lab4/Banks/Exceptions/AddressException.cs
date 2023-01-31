namespace Banks.Exceptions;

public class AddressException : Exception
{
    private AddressException(string message)
        : base(message)
    {
    }

    public static AddressException InvalidHouseNumber(int houseNumber) =>
        new AddressException($"Unable to create address with house number \"{houseNumber}\".");
}