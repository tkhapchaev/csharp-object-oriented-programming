namespace Data.Access.Layer.Exceptions.JsonDataStorageException;

public class JsonDataStorageException : Exception
{
    private JsonDataStorageException(string message)
        : base(message)
    {
    }

    public static JsonDataStorageException PathMustEndWithDirectorySeparatorChar() =>
        new JsonDataStorageException("JSON data storage path must end with directory separator char.");

    public static JsonDataStorageException UnableToDeserializeAccounts() =>
        new JsonDataStorageException("Unable to deserialize accounts.json.");

    public static JsonDataStorageException UnableToDeserializeSmsMessages() =>
        new JsonDataStorageException("Unable to deserialize smsMessages.json.");

    public static JsonDataStorageException UnableToDeserializeEmailMessages() =>
        new JsonDataStorageException("Unable to deserialize emailMessages.json.");

    public static JsonDataStorageException UnableToDeserializeMessengerMessages() =>
        new JsonDataStorageException("Unable to deserialize messengerMessages.json.");

    public static JsonDataStorageException UnableToDeserializeAccountInfos() =>
        new JsonDataStorageException("Unable to deserialize accountInfos.json.");
}