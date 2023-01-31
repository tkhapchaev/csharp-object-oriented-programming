using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTests
{
    private readonly IsuService _isuService;

    public IsuServiceTests()
    {
        _isuService = new IsuService();
    }

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var groupName = new GroupName("M32091");

        Group group = _isuService.AddGroup(groupName);
        Student student = _isuService.AddStudent(group, "Charles Powell");

        Assert.Same(group, student.Group);
        Assert.Contains(student, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var groupName = new GroupName("M32091");

        Group group = _isuService.AddGroup(groupName);

        string[] students =
        {
            "Barbara Smith", "Richard Lawson", "Robert Scott", "Robert Carter", "Alexander Castillo",
            "Ronald Mitchell", "Eileen Goodwin", "Leslie Wallace", "Diana Montgomery", "Patricia Perez",
            "Katherine Parker", "Deborah Wilson", "Ruth Jones", "Katie Moore", "Maxine Garza", "Yolanda Brown",
            "Paul Wright", "Thomas Miller", "John Diaz", "James Campbell", "Evelyn Jackson", "Juan Wright",
            "Elizabeth Hernandez", "Wanda Miller", "Steven Kennedy", "Sandra Pierce", "Stephen Green", "Kim Floyd",
            "Eileen Flores", "Jacqueline Walker", "Ruth Gardner", "Paul Hughes", "Charles Hayes", "Anthony Johnson",
            "Alexander Summers",
        };

        foreach (string student in students)
        {
            _isuService.AddStudent(group, student);
        }

        Assert.Throws<GroupException>(() => _isuService.AddStudent(group, "Jose Tran"));
    }

    [Theory]
    [InlineData("qwerty")]
    [InlineData("1234567890")]
    public void CreateGroupWithInvalidName_ThrowException(string invalidName)
    {
        Assert.Throws<GroupNameException>(() => _isuService.AddGroup(new GroupName(invalidName)));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var oldGroupName = new GroupName("M32081");

        var newGroupName = new GroupName("M32091");

        Group groupBeforeTransfer = _isuService.AddGroup(oldGroupName);
        Group groupAfterTransfer = _isuService.AddGroup(newGroupName);

        Student student = _isuService.AddStudent(groupBeforeTransfer, "Shawn Lawrence");

        _isuService.ChangeStudentGroup(student, groupAfterTransfer);

        Assert.Same(groupAfterTransfer, student.Group);
        Assert.Contains(student, groupAfterTransfer.Students);

        Assert.NotSame(groupBeforeTransfer, student.Group);
        Assert.DoesNotContain(student, groupBeforeTransfer.Students);
    }
}