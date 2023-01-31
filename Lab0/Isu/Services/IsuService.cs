using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly List<Group> _groups;

    private readonly List<Student> _students;

    private readonly int _maxNumberOfMembersInGroup;

    private int _studentId;

    public IsuService()
    {
        _groups = new List<Group>();

        _students = new List<Student>();

        _maxNumberOfMembersInGroup = 35;

        _studentId = 100000;
    }

    public Group AddGroup(GroupName groupName)
    {
        var group = new Group(groupName);

        _groups.Add(group);

        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        if (group.Students.Count >= _maxNumberOfMembersInGroup)
        {
            throw GroupException.ReachedMaxNumberOfMembers(group.GroupName.Name);
        }

        var student = new Student(_studentId++, name, group);

        _students.Add(student);

        group.AddMember(student);

        return student;
    }

    public Student GetStudent(int id)
    {
        foreach (Student student in _students.Where(student => student.Id == id))
        {
            return student;
        }

        throw StudentException.InvalidStudentIdValue(id);
    }

    public Student? FindStudent(int id)
    {
        return _students.Where(student => student.Id == id).Select(student => student).FirstOrDefault();
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return _students.Where(student => student.Group.GroupName.Name == groupName.Name).ToList();
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return _students.Where(student => student.Group.CourseNumber == courseNumber).ToList();
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _groups.Where(group => group.GroupName.Name == groupName.Name).Select(group => group).FirstOrDefault();
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(group => group.CourseNumber == courseNumber).ToList();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (newGroup.Students.Count >= _maxNumberOfMembersInGroup)
        {
            throw GroupException.ReachedMaxNumberOfMembers(newGroup.GroupName.Name);
        }

        Group oldGroup = student.Group;

        oldGroup.RemoveMember(student);

        newGroup.AddMember(student);
    }
}