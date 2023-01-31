using System.Text;
using Backups.Entities;

namespace Backups.Extra.Entities.Logger;

public class FileLogger : ILogger
{
    public FileLogger(IRepository repository, string fileName, bool logWithTimeCode)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        LogWithTimeCode = logWithTimeCode;
    }

    public IRepository Repository { get; }

    public string FileName { get; }

    public bool LogWithTimeCode { get; set; }

    public void Log(string message)
    {
        DateTime currentDateTime = DateTime.Now;

        if (LogWithTimeCode)
        {
            message = $"[{currentDateTime}] {message}";
        }

        using Stream stream = Repository.Append(FileName);
        var unicodeEncoding = new UnicodeEncoding();

        stream.Write(unicodeEncoding.GetBytes(message));
    }
}