namespace Business.Layer.Exceptions.MessageSystemServiceException;

public class MessageSystemServiceException : Exception
{
    private MessageSystemServiceException(string message)
        : base(message)
    {
    }

    public static MessageSystemServiceException NoSuchAccount(string login) =>
        new MessageSystemServiceException($"Couldn't find an account with login \"{login}\".");

    public static MessageSystemServiceException IncorrectPassword(string login) =>
        new MessageSystemServiceException($"Incorrect password for account \"{login}\".");

    public static MessageSystemServiceException NotEnoughRights(string login) =>
        new MessageSystemServiceException($"Account \"{login}\" does not have enough rights to perform this action.");

    public static MessageSystemServiceException UnauthorizedAction() =>
        new MessageSystemServiceException($"This action is only available to authorized users.");

    public static MessageSystemServiceException UnableToLogIn(string login) =>
        new MessageSystemServiceException($"Before this action, you need to log out of your account \"{login}\".");

    public static MessageSystemServiceException UnableToLogOut() =>
        new MessageSystemServiceException($"Before this action, you need to log in your account.");

    public static MessageSystemServiceException UnableToCreateEmptyReport(string login) =>
        new MessageSystemServiceException($"The manager's report created from account \"{login}\" is empty.");

    public static MessageSystemServiceException MessageCannotBeFound(string id) =>
        new MessageSystemServiceException($"The message with the ID \"{id}\" cannot be found.");

    public static MessageSystemServiceException EmployeeCannotBeFound(string id) =>
        new MessageSystemServiceException($"The employee with the ID \"{id}\" cannot be found.");

    public static MessageSystemServiceException LoginMustBeUnique(string login) =>
        new MessageSystemServiceException($"The user with login \"{login}\" already exists.");
}