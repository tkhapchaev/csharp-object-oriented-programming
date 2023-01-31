using Isu.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class OgnpGroup
{
    private readonly List<Student> _students;

    public OgnpGroup(Stream stream)
    {
        Stream = stream ?? throw new ArgumentNullException();

        Schedule = new Schedule();

        _students = new List<Student>();
    }

    public Stream Stream { get; }

    public Schedule Schedule { get; }

    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public void AddMember(Student student)
    {
        if (_students.Contains(student))
        {
            throw OgnpGroupException.OgnpGroupAlreadyContainsSuchMember(Stream.Ognp.Name);
        }

        _students.Add(student);
    }

    public void RemoveMember(Student student)
    {
        if (!_students.Contains(student))
        {
            throw OgnpGroupException.OgnpGroupHasNoSuchMember(Stream.Ognp.Name);
        }

        _students.Remove(student);
    }
}