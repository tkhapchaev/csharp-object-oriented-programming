using Business.Layer.Entities.Account;
using Business.Layer.Entities.Employee;
using Business.Layer.Entities.MessageSource;
using Business.Layer.Entities.Report;
using Business.Layer.Models.AccountInfo;
using Business.Layer.Models.Message;

namespace Business.Layer.Services.MessageSystemService;

public interface IMessageSystemService
{
    IReadOnlyList<AccountInfo> AccountInfos();

    Account AddAccount(Employee owner, string login, string password);

    IMessage AddMessage(IMessageSource messageSource, string contents);

    Account LogIn(string login, string password);

    IReadOnlyList<IMessage> RequestMessages();

    Report MakeReport();

    IMessage GetMessage(string id);

    Employee GetEmployee(string id);

    Employee AddPermission(string employeeId, string permission);

    Employee AddSubordinate(string employeeId, string subordinateId);

    Employee RemoveSubordinate(string employeeId, string subordinateId);

    bool LogOut();
}