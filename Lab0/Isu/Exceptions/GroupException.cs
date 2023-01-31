namespace Isu.Exceptions;

public class GroupException : Exception
{
    private GroupException(string message)
        : base(message)
    {
    }

    public static GroupException GroupAlreadyContainsSuchMember(string groupName) =>
        new GroupException($"Group \"{groupName}\" already contains such member.");

    public static GroupException GroupHasNoSuchMember(string groupName) =>
        new GroupException($"Group \"{groupName}\" has no such member.");

    public static GroupException ReachedMaxNumberOfMembers(string groupName) =>
        new GroupException($"Maximum number of members has been reached in group \"{groupName}\".");
}