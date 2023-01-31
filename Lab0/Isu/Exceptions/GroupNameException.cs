namespace Isu.Exceptions;

public class GroupNameException : Exception
{
    private GroupNameException(string message)
        : base(message)
    {
    }

    public static GroupNameException InvalidGroupName(string groupName) =>
        new GroupNameException($"Unable to create a group named \"{groupName}\".");
}