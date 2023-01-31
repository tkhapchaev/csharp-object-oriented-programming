using Business.Layer.Entities.Employee;
using Business.Layer.Entities.MessageSource;
using Business.Layer.Services.MessageSystemService;
using Data.Access.Layer.Services.DataStorage;

namespace Presentation.Layer.Services.MessageSystemServiceFacade;

public class MessageSystemServiceFacade : IMessageSystemServiceFacade
{
    private readonly JsonDataStorage _jsonDataStorage;

    private MessageSystemService _messageSystemService;

    public MessageSystemServiceFacade()
    {
        _messageSystemService = new MessageSystemService();
        _jsonDataStorage = new JsonDataStorage(Directory.GetCurrentDirectory());
    }

    public void LogIn(string login, string password)
    {
        _messageSystemService.LogIn(login, password);
    }

    public void LogOut()
    {
        _messageSystemService.LogOut();
    }

    public void AddAccount(string owner, string login, string password)
    {
        var employee = new Employee(owner);
        _messageSystemService.AddAccount(employee, login, password);
    }

    public void AddSmsMessage(string source, string contents)
    {
        _messageSystemService.AddMessage(new Phone(source), contents);
    }

    public void AddEmailMessage(string source, string contents)
    {
        _messageSystemService.AddMessage(new Email(source), contents);
    }

    public void AddMessengerMessage(string source, string contents)
    {
        _messageSystemService.AddMessage(new Messenger(source), contents);
    }

    public string RequestMessages()
    {
        return _messageSystemService
            .RequestMessages()
            .Aggregate(string.Empty, (current, message) => current + message + Environment.NewLine);
    }

    public string MakeReport()
    {
        return _messageSystemService.MakeReport().Contents;
    }

    public void MakeMessageReceived(string messageId)
    {
        _messageSystemService.GetMessage(messageId).MakeReceived();
    }

    public void MakeMessageProcessed(string messageId)
    {
        _messageSystemService.GetMessage(messageId).MakeProcessed();
    }

    public void AddPermission(string employeeId, string permission)
    {
        _messageSystemService.AddPermission(employeeId, permission);
    }

    public void AddSubordinate(string employeeId, string subordinateId)
    {
        _messageSystemService.AddSubordinate(employeeId, subordinateId);
    }

    public void RemoveSubordinate(string employeeId, string subordinateId)
    {
        _messageSystemService.RemoveSubordinate(employeeId, subordinateId);
    }

    public string GetUserId()
    {
        return _messageSystemService.CurrentLoggedUser is null
            ? "null"
            : _messageSystemService.CurrentLoggedUser.Owner.Id.ToString();
    }

    public void Save()
    {
        _jsonDataStorage.SaveData(_messageSystemService);
    }

    public void Load()
    {
        _messageSystemService = _jsonDataStorage.LoadData();
    }
}