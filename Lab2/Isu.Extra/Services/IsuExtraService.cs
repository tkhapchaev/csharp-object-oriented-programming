using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;
using Stream = Isu.Extra.Entities.Stream;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private readonly List<MainGroup> _mainGroups;

    private readonly List<OgnpGroup> _ognpGroups;

    private readonly List<Student> _students;

    private readonly List<Ognp> _ognps;

    private readonly IsuService _isuService;

    private readonly int _maxNumberOfMembersInOgnpGroup;

    public IsuExtraService()
    {
        _mainGroups = new List<MainGroup>();

        _ognpGroups = new List<OgnpGroup>();

        _students = new List<Student>();

        _ognps = new List<Ognp>();

        _isuService = new IsuService();

        _maxNumberOfMembersInOgnpGroup = 30;
    }

    public MainGroup AddGroup(GroupName groupName)
    {
        Group group = _isuService.AddGroup(groupName);
        var schedule = new Schedule();

        var mainGroup = new MainGroup(group, schedule);
        _mainGroups.Add(mainGroup);

        return mainGroup;
    }

    public Student AddStudent(MainGroup group, string name)
    {
        Student student = _isuService.AddStudent(group.Group, name);
        _students.Add(student);

        return student;
    }

    public void ChangeStudentGroup(Student student, MainGroup newGroup)
    {
        _isuService.ChangeStudentGroup(student, newGroup.Group);
    }

    public Ognp AddOgnp(string ognpName, FacultyName facultyName)
    {
        var ognp = new Ognp(ognpName, facultyName);
        _ognps.Add(ognp);

        return ognp;
    }

    public Stream AddOgnpStream(Ognp ognp, Teacher teacher)
    {
        var stream = new Stream(ognp, teacher);
        ognp.AddStream(stream);

        return stream;
    }

    public OgnpGroup AddOgnpGroup(Stream stream)
    {
        var ognpGroup = new OgnpGroup(stream);
        stream.AddGroup(ognpGroup);
        _ognpGroups.Add(ognpGroup);

        return ognpGroup;
    }

    public void EnrollStudentInOgnp(Student student, Ognp ognp)
    {
        Group group = student.Group;

        char firstGroupNameLetter = group.GroupName.Name[0];
        var facultyName = (FacultyName)firstGroupNameLetter;

        if (facultyName == ognp.Faculty)
        {
            throw OgnpException.OgnpFacultyCoincidesWithTheMainFaculty(ognp.Name);
        }

        Schedule? schedule = _mainGroups
            .LastOrDefault(timetable => group.GroupName.Name == timetable.Group.GroupName.Name)
            ?.Schedule;

        ArgumentNullException.ThrowIfNull(schedule);

        OgnpGroup? targetOgnpGroup = null;
        int numberOfOgnpGroups = 0, numberOfEnrolledStudents = 0;

        foreach (Stream stream in ognp.Streams)
        {
            numberOfOgnpGroups += stream.Groups.Count;
            numberOfEnrolledStudents += stream.Groups.Sum(ognpGroup => ognpGroup.Students.Count);
        }

        if (_maxNumberOfMembersInOgnpGroup * numberOfOgnpGroups <= numberOfEnrolledStudents)
        {
            throw OgnpException.ReachedMaximumNumberOfStudents(ognp.Name);
        }

        foreach (Stream stream in ognp.Streams)
        {
            targetOgnpGroup = stream.Groups.LastOrDefault(ognpGroup =>
                !CheckSchedulesForIntersections(schedule, ognpGroup.Schedule) &&
                ognpGroup.Students.Count < _maxNumberOfMembersInOgnpGroup);
        }

        if (targetOgnpGroup is null)
        {
            throw OgnpException.CantFindAppropriateGroup(ognp.Name);
        }

        targetOgnpGroup.AddMember(student);
    }

    public void DisenrollStudentFromOgnp(Student student)
    {
        foreach (OgnpGroup ognpGroup in _ognpGroups.Where(ognpGroup =>
                     ognpGroup.Students.Any(member => student == member)))
        {
            ognpGroup.RemoveMember(student);

            return;
        }
    }

    public Ognp GetOgnp(string ognpName)
    {
        Ognp? targetOgnp = _ognps.LastOrDefault(ognp => ognp.Name == ognpName);

        if (targetOgnp is null)
        {
            throw OgnpException.ThereIsNoSuchOgnp(ognpName);
        }

        return targetOgnp;
    }

    public List<Stream> GetOgnpStreams(string ognpName)
    {
        return _ognps.Where(ognp => ognp.Name == ognpName).SelectMany(ognp => ognp.Streams).ToList();
    }

    public List<Student> GetStudentsFromOgnpGroup(OgnpGroup ognpGroup)
    {
        return ognpGroup.Students.ToList();
    }

    public List<Student> GetNonEnrolledStudents()
    {
        var nonEnrolledStudents = new List<Student>();

        foreach (Student student in _students)
        {
            bool studentIsEnrolledInOgnp = _ognpGroups.Any(ognpGroup => ognpGroup.Students.Contains(student));

            if (!studentIsEnrolledInOgnp)
            {
                nonEnrolledStudents.Add(student);
            }
        }

        return nonEnrolledStudents;
    }

    private bool CheckSchedulesForIntersections(Schedule mainSchedule, Schedule ognpSchedule)
    {
        return mainSchedule.Lessons.Any(lesson => ognpSchedule.Lessons.Any(ognpLesson =>
            lesson.LessonNumber == ognpLesson.LessonNumber && lesson.DayOfWeek == ognpLesson.DayOfWeek));
    }
}