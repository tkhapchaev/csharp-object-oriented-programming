namespace Backups.Extra.Exceptions;

public class JsonConfigurationException : Exception
{
    private JsonConfigurationException(string message)
        : base(message)
    {
    }

    public static JsonConfigurationException CannotDeserializeBackupTaskConfiguration() =>
        new JsonConfigurationException($"An error occurred during .json configuration deserialization.");
}