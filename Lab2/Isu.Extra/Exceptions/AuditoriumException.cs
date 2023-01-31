namespace Isu.Extra.Exceptions;

public class AuditoriumException : Exception
{
    private AuditoriumException(string message)
        : base(message)
    {
    }

    public static AuditoriumException InvalidAuditoriumNumber(int number) =>
        new AuditoriumException($"Invalid auditorium number: \"{number}\"");
}