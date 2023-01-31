namespace Isu.Exceptions;

public class StudentException : Exception
{
    private StudentException(string message)
        : base(message)
    {
    }

    public static StudentException InvalidStudentIdValue(int studentId) =>
        new StudentException($"Invalid student ID value: \"{studentId}\"");

    public static StudentException InvalidStudentName(string studentName) =>
        new StudentException($"Unable to create a student with the name \"{studentName}\".");
}