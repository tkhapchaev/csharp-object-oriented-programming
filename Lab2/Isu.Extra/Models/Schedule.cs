using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Schedule
{
    private readonly List<Lesson> _lessons;

    public Schedule()
    {
        _lessons = new List<Lesson>();
    }

    public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

    public void AddLesson(Lesson lesson)
    {
        if (_lessons.Contains(lesson))
        {
            throw ScheduleException.ScheduleAlreadyContainsSuchLesson(lesson.Name);
        }

        _lessons.Add(lesson);
    }

    public void RemoveLesson(Lesson lesson)
    {
        if (!_lessons.Contains(lesson))
        {
            throw ScheduleException.ScheduleHasNoSuchLesson(lesson.Name);
        }

        _lessons.Remove(lesson);
    }
}