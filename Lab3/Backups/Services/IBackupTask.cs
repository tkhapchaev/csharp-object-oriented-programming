using Backups.Models;

namespace Backups.Services;

public interface IBackupTask
{
    void AddBackupObject(BackupObject backupObject);

    void RemoveBackupObject(BackupObject backupObject);

    void NewBackup();
}