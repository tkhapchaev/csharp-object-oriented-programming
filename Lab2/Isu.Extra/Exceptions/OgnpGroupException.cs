namespace Isu.Extra.Exceptions;

public class OgnpGroupException : Exception
{
    private OgnpGroupException(string message)
        : base(message)
    {
    }

    public static OgnpGroupException OgnpGroupAlreadyContainsSuchMember(string ognpName) =>
        new OgnpGroupException($"\"{ognpName}\" group already contains such member.");

    public static OgnpGroupException OgnpGroupHasNoSuchMember(string ognpName) =>
        new OgnpGroupException($"\"{ognpName}\" group has no such member.");

    public static OgnpGroupException ReachedMaxNumberOfMembers(string ognpName) =>
        new OgnpGroupException($"Maximum number of members has been reached in \"{ognpName}\" group.");
}