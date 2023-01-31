namespace Isu.Extra.Exceptions;

public class StreamException : Exception
{
    private StreamException(string message)
        : base(message)
    {
    }

    public static StreamException StreamAlreadyContainsSuchGroup(string ognpName) =>
        new StreamException($"\"{ognpName}\" stream already contains such group.");

    public static StreamException StreamHasNoSuchGroup(string ognpName) =>
        new StreamException($"\"{ognpName}\" stream does not contain such group.");
}