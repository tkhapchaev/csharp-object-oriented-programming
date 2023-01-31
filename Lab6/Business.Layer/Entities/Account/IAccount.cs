namespace Business.Layer.Entities.Account;

public interface IAccount
{
    Employee.Employee Owner { get; }

    string Login { get; }
}