namespace Presentation.Layer.Services.MessageSystemServiceFacade;

public interface IMessageSystemServiceFacade
{
    void LogIn(string login, string password);

    void LogOut();

    void AddAccount(string owner, string login, string password);

    void AddSmsMessage(string source, string contents);

    void AddEmailMessage(string source, string contents);

    void AddMessengerMessage(string source, string contents);

    string RequestMessages();

    string MakeReport();

    void MakeMessageReceived(string messageId);

    void MakeMessageProcessed(string messageId);

    void AddPermission(string employeeId, string permission);

    void AddSubordinate(string employeeId, string subordinateId);

    void RemoveSubordinate(string employeeId, string subordinateId);

    string GetUserId();

    void Save();

    void Load();
}