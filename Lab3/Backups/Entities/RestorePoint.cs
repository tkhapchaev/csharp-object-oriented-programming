using Backups.Models;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<BackupObject> _backupObjects;

    public RestorePoint(DateTime creationDate)
    {
        CreationDate = creationDate;
        _backupObjects = new List<BackupObject>();
    }

    public DateTime CreationDate { get; private set; }

    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects.AsReadOnly();

    public void AddBackupObject(BackupObject backupObject)
    {
        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        _backupObjects.Remove(backupObject);
    }

    public void ChangeCreationDate(DateTime creationDate)
    {
        CreationDate = creationDate;
    }
}