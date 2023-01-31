namespace Backups.Extra.Entities.Logger;

public interface ILogger
{
    public bool LogWithTimeCode { get; set; }

    void Log(string message);
}