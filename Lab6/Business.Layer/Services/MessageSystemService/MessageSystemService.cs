using Business.Layer.Entities.Account;
using Business.Layer.Entities.Employee;
using Business.Layer.Entities.MessageSource;
using Business.Layer.Entities.Report;
using Business.Layer.Exceptions.MessageSystemServiceException;
using Business.Layer.Models.AccountInfo;
using Business.Layer.Models.Message;
using Business.Layer.Models.PasswordHashingAlgorithm;

namespace Business.Layer.Services.MessageSystemService;

public class MessageSystemService : IMessageSystemService
{
    private readonly List<Account> _accounts;

    private readonly List<IMessage> _messages;

    private readonly Dictionary<string, string> _accountInfos;

    public MessageSystemService()
    {
        _accounts = new List<Account>();
        _messages = new List<IMessage>();
        _accountInfos = new Dictionary<string, string>();
        CurrentLoggedUser = null;

        var root = new Employee("root");
        root.AddPermission("SMS");
        root.AddPermission("E-mail");
        root.AddPermission("Messenger");
        root.AddPermission("AddAccount");
        root.AddPermission("AddMessage");
        root.AddPermission("GetMessage");
        root.AddPermission("GetEmployee");
        root.AddPermission("AddPermission");
        root.AddPermission("AddSubordinate");
        root.AddPermission("RemoveSubordinate");

        AddAccount(root, "root", "admin");
    }

    public MessageSystemService(List<Account> accounts, List<IMessage> messages, List<AccountInfo> accountInfos)
    {
        _accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
        _messages = messages ?? throw new ArgumentNullException(nameof(messages));
        _accountInfos = new Dictionary<string, string>();

        foreach (AccountInfo accountInfo in accountInfos)
        {
            _accountInfos[accountInfo.Login] = accountInfo.Password;
        }

        CurrentLoggedUser = null;
    }

    public Account? CurrentLoggedUser { get; private set; }

    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();

    public IReadOnlyList<IMessage> Messages => _messages.AsReadOnly();

    public IReadOnlyList<AccountInfo> AccountInfos()
    {
        return _accountInfos.Select(accountInfo => new AccountInfo(accountInfo.Key, accountInfo.Value)).ToList()
            .AsReadOnly();
    }

    public Account AddAccount(Employee owner, string login, string password)
    {
        var passwordHashingAlgorithm = new PasswordHashingAlgorithm();

        if (owner.Name == "root" && login == "root" && password == "admin")
        {
            var root = new Account(owner, login);

            _accounts.Add(root);
            _accountInfos[login] = passwordHashingAlgorithm.GetHash(password);

            return root;
        }

        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("AddAccount") && CurrentLoggedUser.Login != "root")
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        if (_accountInfos.ContainsKey(login))
        {
            throw MessageSystemServiceException.LoginMustBeUnique(login);
        }

        var account = new Account(owner, login);

        _accounts.Add(account);
        _accountInfos[login] = passwordHashingAlgorithm.GetHash(password);

        return account;
    }

    public IMessage AddMessage(IMessageSource messageSource, string contents)
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("AddMessage"))
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        IMessage message = messageSource.NewMessage(contents);
        _messages.Add(message);

        return message;
    }

    public Account LogIn(string login, string password)
    {
        var passwordHashingAlgorithm = new PasswordHashingAlgorithm();
        string passwordHash = passwordHashingAlgorithm.GetHash(password);

        Account account = _accounts.FirstOrDefault(account => login == account.Login) ??
                          throw MessageSystemServiceException.NoSuchAccount(login);

        if (CurrentLoggedUser is not null)
        {
            throw MessageSystemServiceException.UnableToLogIn(login);
        }

        if (passwordHash != _accountInfos[login])
        {
            throw MessageSystemServiceException.IncorrectPassword(login);
        }

        CurrentLoggedUser = account;
        return account;
    }

    public IReadOnlyList<IMessage> RequestMessages()
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        foreach (IMessage message in _messages
                     .Where(message => CurrentLoggedUser.Owner.Permissions.Contains(message.MessageType)))
        {
            message.MakeReceived();
            CurrentLoggedUser.AddMessage(message);
        }

        return CurrentLoggedUser.Messages;
    }

    public Report MakeReport()
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (CurrentLoggedUser.Owner.Subordinates.Count == 0)
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        var messages = new List<IMessage>();

        foreach (Employee employee in CurrentLoggedUser.Owner.Subordinates)
        {
            Account employeeAccount = _accounts.FirstOrDefault(account => employee == account.Owner) ??
                                      throw MessageSystemServiceException.NoSuchAccount(employee.Name);

            messages.AddRange(
                employeeAccount.Messages.Where(message => message.MessageStatus == MessageStatus.Processed));
        }

        if (messages.Count == 0)
        {
            throw MessageSystemServiceException.UnableToCreateEmptyReport(CurrentLoggedUser.Login);
        }

        var report = new Report();
        report.MakeReport(messages);

        return report;
    }

    public IMessage GetMessage(string id)
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("GetMessage"))
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        return _messages.FirstOrDefault(message => id == message.Id.ToString()) ??
               throw MessageSystemServiceException.MessageCannotBeFound(id);
    }

    public Employee GetEmployee(string id)
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("GetEmployee"))
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        Account account = _accounts.FirstOrDefault(account => id == account.Owner.Id.ToString()) ??
                          throw MessageSystemServiceException.EmployeeCannotBeFound(id);

        return account.Owner;
    }

    public Employee AddPermission(string employeeId, string permission)
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("AddPermission") && CurrentLoggedUser.Login != "root")
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        Employee employee = GetEmployee(employeeId);
        employee.AddPermission(permission);

        return employee;
    }

    public Employee AddSubordinate(string employeeId, string subordinateId)
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("AddSubordinate"))
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        Employee employee = GetEmployee(employeeId);
        employee.AddSubordinate(GetEmployee(subordinateId));

        return employee;
    }

    public Employee RemoveSubordinate(string employeeId, string subordinateId)
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnauthorizedAction();
        }

        if (!CurrentLoggedUser.Owner.Permissions.Contains("RemoveSubordinate"))
        {
            throw MessageSystemServiceException.NotEnoughRights(CurrentLoggedUser.Login);
        }

        Employee employee = GetEmployee(employeeId);
        employee.RemoveSubordinate(GetEmployee(subordinateId));

        return employee;
    }

    public bool LogOut()
    {
        if (CurrentLoggedUser is null)
        {
            throw MessageSystemServiceException.UnableToLogOut();
        }

        CurrentLoggedUser = null;

        return true;
    }
}