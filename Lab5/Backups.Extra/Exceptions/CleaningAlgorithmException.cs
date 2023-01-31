namespace Backups.Extra.Exceptions;

public class CleaningAlgorithmException : Exception
{
    private CleaningAlgorithmException(string message)
        : base(message)
    {
    }

    public static CleaningAlgorithmException InvalidLimit(int amount) =>
        new CleaningAlgorithmException($"Invalid limit for the number of restore points: \"{amount}\".");
}