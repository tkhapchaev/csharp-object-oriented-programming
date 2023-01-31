using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;
using Stream = Isu.Extra.Entities.Stream;

namespace Isu.Extra.Test;

public class IsuExtraServiceTests
{
    private readonly IsuExtraService _isuExtraService;

    public IsuExtraServiceTests()
    {
        _isuExtraService = new IsuExtraService();
    }

    [Fact]
    public void AddOgnp_OgnpWasSuccessfullyCreated()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Assert.Same(_isuExtraService.GetOgnp("Applied Mathematics and Programming"), ognp);
    }

    [Fact]
    public void EnrollStudentInOgnp_StudentIsSuccessfullyEnrolled()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Stream stream = _isuExtraService.AddOgnpStream(ognp, new Teacher("Teacher teacher"));

        OgnpGroup ognpGroup = _isuExtraService.AddOgnpGroup(stream);

        MainGroup mainGroup = _isuExtraService.AddGroup(new GroupName("P32001"));

        Student student = _isuExtraService.AddStudent(mainGroup, "Student student");

        _isuExtraService.EnrollStudentInOgnp(student, ognp);

        Assert.Contains(student, ognpGroup.Students);
    }

    [Fact]
    public void EnrollStudentInOgnpOfHisFaculty_ThrowException()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Stream stream = _isuExtraService.AddOgnpStream(ognp, new Teacher("Teacher teacher"));

        _isuExtraService.AddOgnpGroup(stream);

        MainGroup mainGroup = _isuExtraService.AddGroup(new GroupName("M32091"));

        Student student = _isuExtraService.AddStudent(mainGroup, "Student student");

        Assert.Throws<OgnpException>(() => _isuExtraService.EnrollStudentInOgnp(student, ognp));
    }

    [Fact]
    public void OgnpHasNoFreePlaces_ThrowException()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Stream stream = _isuExtraService.AddOgnpStream(ognp, new Teacher("Teacher teacher"));

        _isuExtraService.AddOgnpGroup(stream);

        MainGroup mainGroup = _isuExtraService.AddGroup(new GroupName("P32001"));

        string[] students =
        {
            "Barbara Smith", "Richard Lawson", "Robert Scott", "Robert Carter", "Alexander Castillo",
            "Ronald Mitchell", "Eileen Goodwin", "Leslie Wallace", "Diana Montgomery", "Patricia Perez",
            "Katherine Parker", "Deborah Wilson", "Ruth Jones", "Katie Moore", "Maxine Garza", "Yolanda Brown",
            "Paul Wright", "Thomas Miller", "John Diaz", "James Campbell", "Evelyn Jackson", "Juan Wright",
            "Elizabeth Hernandez", "Wanda Miller", "Steven Kennedy", "Sandra Pierce", "Stephen Green", "Kim Floyd",
            "Eileen Flores", "Jacqueline Walker",
        };

        foreach (string member in students)
        {
            Student student = _isuExtraService.AddStudent(mainGroup, member);
            _isuExtraService.EnrollStudentInOgnp(student, ognp);
        }

        Student extraStudent = _isuExtraService.AddStudent(mainGroup, "Jose Tran");

        Assert.Throws<OgnpException>(() => _isuExtraService.EnrollStudentInOgnp(extraStudent, ognp));
    }

    [Fact]
    public void DisenrollStudentFromOgnp_StudentIsSuccessfullyDisenrolled()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Stream stream = _isuExtraService.AddOgnpStream(ognp, new Teacher("Teacher teacher"));

        OgnpGroup ognpGroup = _isuExtraService.AddOgnpGroup(stream);

        MainGroup mainGroup = _isuExtraService.AddGroup(new GroupName("P32001"));

        Student student = _isuExtraService.AddStudent(mainGroup, "Student student");

        _isuExtraService.DisenrollStudentFromOgnp(student);

        Assert.DoesNotContain(student, ognpGroup.Students);
    }

    [Fact]
    public void GetListOfNonEnrolledInOgnpStudent_ListIsCorrect()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Stream stream = _isuExtraService.AddOgnpStream(ognp, new Teacher("Teacher teacher"));

        _isuExtraService.AddOgnpGroup(stream);

        MainGroup mainGroup = _isuExtraService.AddGroup(new GroupName("P32001"));

        string[] students =
        {
            "Barbara Smith", "Richard Lawson", "Robert Scott", "Robert Carter", "Alexander Castillo",
            "Ronald Mitchell", "Eileen Goodwin", "Leslie Wallace", "Diana Montgomery", "Patricia Perez",
            "Katherine Parker", "Deborah Wilson", "Ruth Jones", "Katie Moore", "Maxine Garza", "Yolanda Brown",
            "Paul Wright", "Thomas Miller", "John Diaz", "James Campbell", "Evelyn Jackson", "Juan Wright",
            "Elizabeth Hernandez", "Wanda Miller", "Steven Kennedy", "Sandra Pierce", "Stephen Green", "Kim Floyd",
            "Eileen Flores",
        };

        foreach (string member in students)
        {
            Student student = _isuExtraService.AddStudent(mainGroup, member);
            _isuExtraService.EnrollStudentInOgnp(student, ognp);
        }

        Student newStudent = _isuExtraService.AddStudent(mainGroup, "Jose Tran");

        Assert.Contains(newStudent, _isuExtraService.GetNonEnrolledStudents());
    }

    [Fact]
    public void OgnpIntersectsWithTheMainSchedule_ThrowException()
    {
        Ognp ognp = _isuExtraService.AddOgnp(
            "Applied Mathematics and Programming",
            FacultyName.InformationTechnologyAndProgramming);

        Stream stream = _isuExtraService.AddOgnpStream(ognp, new Teacher("Teacher teacher"));

        OgnpGroup ognpGroup = _isuExtraService.AddOgnpGroup(stream);

        MainGroup mainGroup = _isuExtraService.AddGroup(new GroupName("M32091"));

        Student student = _isuExtraService.AddStudent(mainGroup, "Student student");

        mainGroup.Schedule.AddLesson(new Lesson(
            "Lesson",
            DayOfWeek.Monday,
            LessonNumber.Second,
            new Teacher("Teacher teacher"),
            new Auditorium(239)));

        ognpGroup.Schedule.AddLesson(new Lesson(
            "Lesson",
            DayOfWeek.Monday,
            LessonNumber.Second,
            new Teacher("Teacher teacher"),
            new Auditorium(409)));

        Assert.Throws<OgnpException>(() => _isuExtraService.EnrollStudentInOgnp(student, ognp));
    }
}