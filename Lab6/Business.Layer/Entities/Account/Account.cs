using Business.Layer.Models.Message;

namespace Business.Layer.Entities.Account;

public class Account : IAccount
{
    private readonly List<IMessage> _messages;

    public Account(Employee.Employee owner, string login)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        Login = login ?? throw new ArgumentNullException(nameof(login));
        _messages = new List<IMessage>();
    }

    public Employee.Employee Owner { get; }

    public string Login { get; }

    public IReadOnlyList<IMessage> Messages => _messages.AsReadOnly();

    public void AddMessage(IMessage message)
    {
        _messages.Add(message);
    }

    public void RemoveMessage(IMessage message)
    {
        _messages.Remove(message);
    }
}