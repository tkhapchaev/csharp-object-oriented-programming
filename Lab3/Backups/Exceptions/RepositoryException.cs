namespace Backups.Exceptions;

public class RepositoryException : Exception
{
    private RepositoryException(string message)
        : base(message)
    {
    }

    public static RepositoryException PathShouldEndWithDirectorySeparatorChar() =>
        throw new RepositoryException($"Path should end with a directory separator char.");

    public static RepositoryException InvalidPath() =>
        throw new RepositoryException($"Invalid path.");
}