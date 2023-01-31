namespace Isu.Extra.Exceptions;

public class OgnpException : Exception
{
    private OgnpException(string message)
        : base(message)
    {
    }

    public static OgnpException OgnpAlreadyContainsSuchStream(string ognpName) =>
        new OgnpException($"\"{ognpName}\" already contains such stream.");

    public static OgnpException OgnpHasNoSuchStream(string ognpName) =>
        new OgnpException($"\"{ognpName}\" does not contain such stream.");

    public static OgnpException ThereIsNoSuchOgnp(string ognpName) =>
        new OgnpException($"OGNP \"{ognpName}\" does not exist.");

    public static OgnpException OgnpFacultyCoincidesWithTheMainFaculty(string ognpName) =>
        new OgnpException($"The faculty of OGNP \"{ognpName}\" is a main study faculty for this student.");

    public static OgnpException ReachedMaximumNumberOfStudents(string ognpName) =>
        new OgnpException($"All OGNP \"{ognpName}\" streams and groups are full.");

    public static OgnpException CantFindAppropriateGroup(string ognpName) =>
        new OgnpException($"Unable to enroll in OGNP \"{ognpName}\".");
}