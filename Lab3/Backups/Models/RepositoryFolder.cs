using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class RepositoryFolder : IRepositoryObject
{
    private readonly string _path;

    public RepositoryFolder(string path, IStorageAlgorithm storageAlgorithm)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
        StorageAlgorithm = storageAlgorithm ?? throw new ArgumentNullException(nameof(storageAlgorithm));
    }

    public IStorageAlgorithm StorageAlgorithm { get; }

    public string GetName()
    {
        return Path.GetFileName(Path.GetDirectoryName(_path)) ?? throw RepositoryException.InvalidPath();
    }

    public string GetPath() => _path;

    public void Write(BackupObject backupObject, IRepository repository, string archiveName)
    {
        StorageAlgorithm.WriteStorageFolder(backupObject, repository, archiveName);
    }
}