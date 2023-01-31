namespace Isu.Extra.Exceptions;

public class TeacherException : Exception
{
    private TeacherException(string message)
        : base(message)
    {
    }

    public static TeacherException InvalidTeacherName(string name) =>
        new TeacherException($"Invalid teacher name: \"{name}\"");
}