namespace Backups.Models;

public class BackupObject : IBackupObject
{
    public BackupObject(string path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public string Path { get; }

    public string GetName()
    {
        char directorySeparatorChar = System.IO.Path.DirectorySeparatorChar;

        if (Path.Contains('/'))
        {
            directorySeparatorChar = '/';
        }

        if (Path.Contains('\\'))
        {
            directorySeparatorChar = '\\';
        }

        var path = Path.Split(directorySeparatorChar).ToList();

        if (path.Last() == string.Empty)
        {
            path.Remove(path.Last());
        }

        return path.Last();
    }
}