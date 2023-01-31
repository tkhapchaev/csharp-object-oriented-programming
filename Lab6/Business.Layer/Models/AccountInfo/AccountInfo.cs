namespace Business.Layer.Models.AccountInfo;

public class AccountInfo
{
    public AccountInfo(string login, string password)
    {
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    public string Login { get; }

    public string Password { get; }
}