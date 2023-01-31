using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private readonly List<Student> _students;

    public Group(GroupName groupName)
    {
        const int courseNumberPosition = 2;

        GroupName = groupName ?? throw new ArgumentNullException();

        CourseNumber = (CourseNumber)GroupName.Name[courseNumberPosition];

        _students = new List<Student>();
    }

    public GroupName GroupName { get; }

    public CourseNumber CourseNumber { get; }

    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public void AddMember(Student student)
    {
        if (_students.Contains(student))
        {
            throw GroupException.GroupAlreadyContainsSuchMember(student.Name);
        }

        _students.Add(student);

        student.Group = this;
    }

    public void RemoveMember(Student student)
    {
        if (!_students.Contains(student))
        {
            throw GroupException.GroupHasNoSuchMember(student.Name);
        }

        _students.Remove(student);
    }
}