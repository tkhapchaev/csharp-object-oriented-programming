using Backups.Entities;

namespace Backups.Models;

public interface IRepositoryObject
{
    string GetName();

    string GetPath();

    void Write(BackupObject backupObject, IRepository repository, string archiveName);
}