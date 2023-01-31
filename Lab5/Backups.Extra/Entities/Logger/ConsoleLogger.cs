namespace Backups.Extra.Entities.Logger;

public class ConsoleLogger : ILogger
{
    public ConsoleLogger(bool logWithTimeCode)
    {
        LogWithTimeCode = logWithTimeCode;
    }

    public bool LogWithTimeCode { get; set; }

    public void Log(string message)
    {
        DateTime currentDateTime = DateTime.Now;

        if (LogWithTimeCode)
        {
            message = $"[{currentDateTime}] {message}";
        }

        Console.WriteLine(message);
    }
}