using Backups.Entities;

namespace Backups.Models;

public interface IStorageAlgorithm
{
    List<IStorage> Store(RestorePoint restorePoint);

    void WriteStorageFile(BackupObject backupObject, IRepository repository, string archiveName);

    void WriteStorageFolder(BackupObject backupObject, IRepository repository, string archiveName);
}