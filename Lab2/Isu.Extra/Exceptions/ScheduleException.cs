namespace Isu.Extra.Exceptions;

public class ScheduleException : Exception
{
    private ScheduleException(string message)
        : base(message)
    {
    }

    public static ScheduleException ScheduleAlreadyContainsSuchLesson(string lessonName) =>
        new ScheduleException($"Schedule already contains \"{lessonName}\" lesson.");

    public static ScheduleException ScheduleHasNoSuchLesson(string lessonName) =>
        new ScheduleException($"Schedule does not contain \"{lessonName}\" lesson.");
}