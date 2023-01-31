using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Lesson
{
    public Lesson(
        string lessonName,
        DayOfWeek dayOfWeek,
        LessonNumber lessonNumber,
        Teacher teacher,
        Auditorium auditorium)
    {
        Name = lessonName ?? throw new ArgumentNullException();

        DayOfWeek = dayOfWeek;

        LessonNumber = lessonNumber;

        Teacher = teacher ?? throw new ArgumentNullException();

        Auditorium = auditorium ?? throw new ArgumentNullException();
    }

    public string Name { get; }

    public DayOfWeek DayOfWeek { get; }

    public LessonNumber LessonNumber { get; }

    public Teacher Teacher { get; }

    public Auditorium Auditorium { get; }
}