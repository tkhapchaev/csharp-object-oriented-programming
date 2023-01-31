namespace Banks.Exceptions;

public class PassportException : Exception
{
    private PassportException(string message)
        : base(message)
    {
    }

    public static PassportException InvalidSeries(int series) =>
        new PassportException($"Unable to create passport with series \"{series}\".");

    public static PassportException InvalidNumber(int number) =>
        new PassportException($"Unable to create passport with number \"{number}\".");
}