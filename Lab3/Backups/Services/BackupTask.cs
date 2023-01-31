using Backups.Entities;
using Backups.Models;

namespace Backups.Services;

public class BackupTask : IBackupTask
{
    private readonly List<BackupObject> _backupObjects;

    public BackupTask(
        string name,
        IStorageWriter storageWriter,
        IStorageAlgorithm storageAlgorithm,
        IRepository repository)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        StorageWriter = storageWriter ?? throw new ArgumentNullException(nameof(storageWriter));
        StorageAlgorithm = storageAlgorithm ?? throw new ArgumentNullException(nameof(storageAlgorithm));
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));

        Repository.SetInternalDirectory(name);

        Backup = new Backup();
        _backupObjects = new List<BackupObject>();
    }

    public string Name { get; }

    public IStorageWriter StorageWriter { get; }

    public IStorageAlgorithm StorageAlgorithm { get; }

    public IRepository Repository { get; }

    public Backup Backup { get; }

    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects.AsReadOnly();

    public void AddBackupObject(BackupObject backupObject)
    {
        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        _backupObjects.Remove(backupObject);
    }

    public void NewBackup()
    {
        var restorePoint = new RestorePoint(DateTime.Now);

        foreach (BackupObject backupObject in _backupObjects)
        {
            restorePoint.AddBackupObject(backupObject);
        }

        Backup.AddRestorePoint(restorePoint);

        StorageWriter.Write(StorageAlgorithm.Store(restorePoint), Repository);
    }
}