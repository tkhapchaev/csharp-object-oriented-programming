using Business.Layer.Entities.Account;
using Business.Layer.Entities.Employee;
using Business.Layer.Entities.MessageSource;
using Business.Layer.Entities.Report;
using Business.Layer.Exceptions.MessageSystemServiceException;
using Business.Layer.Models.Message;
using Business.Layer.Services.MessageSystemService;
using Xunit;

namespace Test.MessageSystemServiceTest;

public class MessageSystemServiceTest
{
    private readonly MessageSystemService _messageSystemService;

    public MessageSystemServiceTest()
    {
        _messageSystemService = new MessageSystemService();
    }

    [Fact]
    public void RegisterNewAccount_AccountWasSuccessfullyRegistered()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");

        var owners = _messageSystemService.Accounts.Select(account => employee).ToList();

        Assert.Contains(employee, owners);
    }

    [Fact]
    public void LogInAccount_SuccessAuthentication()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.LogOut();

        Assert.Same(_messageSystemService.LogIn("employee", "Qwerty123").Owner, employee);
    }

    [Fact]
    public void SourceSendsMessage_MessageWasReceivedBySystem()
    {
        _messageSystemService.LogIn("root", "admin");
        IMessage message = _messageSystemService.AddMessage(new Phone("900"), "Message");

        Assert.Contains(message, _messageSystemService.Messages);
    }

    [Fact]
    public void RequestMessages_ListOfMessagesHasBeenGenerated()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        Account account = _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        IMessage message = _messageSystemService.AddMessage(new Email("test@gmail.com"), "Message!");
        employee.AddPermission("E-mail");
        _messageSystemService.LogOut();
        _messageSystemService.LogIn("employee", "Qwerty123");
        _messageSystemService.RequestMessages();

        Assert.Contains(message, account.Messages);
    }

    [Fact]
    public void ManagerMakesReport_ReportHasBeenGenerated()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        IMessage message = _messageSystemService.AddMessage(new Email("test@gmail.com"), "Message!");
        employee.AddPermission("E-mail");
        _messageSystemService.LogOut();

        var manager = new Employee("Manager");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(manager, "manager", "Qwerty12345");
        _messageSystemService.LogOut();

        _messageSystemService.LogIn("employee", "Qwerty123");
        _messageSystemService.RequestMessages();
        _messageSystemService.LogOut();

        _messageSystemService.LogIn("manager", "Qwerty12345");
        manager.AddSubordinate(employee);
        message.MakeProcessed();

        Report report = _messageSystemService.MakeReport();

        Assert.Contains("test@gmail.com", report.Contents);
    }

    [Fact]
    public void ManagerMakesEmptyReport_ThrowException()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.AddMessage(new Email("test@gmail.com"), "Message!");
        _messageSystemService.LogOut();
        employee.AddPermission("E-mail");

        var manager = new Employee("Manager");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(manager, "manager", "Qwerty12345");
        _messageSystemService.LogOut();
        _messageSystemService.LogIn("manager", "Qwerty12345");
        manager.AddSubordinate(employee);

        Assert.Throws<MessageSystemServiceException>(() => _messageSystemService.MakeReport());
    }

    [Fact]
    public void UnauthorizedAction_ThrowException()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.LogOut();

        Assert.Throws<MessageSystemServiceException>(() => _messageSystemService.RequestMessages());
    }

    [Fact]
    public void OrdinaryEmployeeIsTryingToMakeReport_ThrowException()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.AddMessage(new Email("test@gmail.com"), "Message!");
        employee.AddPermission("E-mail");
        _messageSystemService.LogOut();
        _messageSystemService.LogIn("employee", "Qwerty123");

        Assert.Throws<MessageSystemServiceException>(() => _messageSystemService.MakeReport());
    }

    [Fact]
    public void UserEntersIncorrectPassword_ThrowException()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.LogOut();
        _messageSystemService.LogIn("employee", "Qwerty123");

        Assert.Throws<MessageSystemServiceException>(() => _messageSystemService.LogIn("employee", "Qwerty"));
    }

    [Fact]
    public void LogInWhileLoggedIn_ThrowException()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.LogOut();
        _messageSystemService.LogIn("employee", "Qwerty123");

        Assert.Throws<MessageSystemServiceException>(() => _messageSystemService.LogIn("employee", "Qwerty123"));
    }

    [Fact]
    public void LogOutWithoutLoggingIn_ThrowException()
    {
        var employee = new Employee("Employee");
        _messageSystemService.LogIn("root", "admin");
        _messageSystemService.AddAccount(employee, "employee", "Qwerty123");
        _messageSystemService.LogOut();

        Assert.Throws<MessageSystemServiceException>(() => _messageSystemService.LogOut());
    }
}