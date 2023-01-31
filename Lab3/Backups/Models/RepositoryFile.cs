using Backups.Entities;

namespace Backups.Models;

public class RepositoryFile : IRepositoryObject
{
    private readonly string _path;

    public RepositoryFile(string path, IStorageAlgorithm storageAlgorithm)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
        StorageAlgorithm = storageAlgorithm ?? throw new ArgumentNullException(nameof(storageAlgorithm));
    }

    public IStorageAlgorithm StorageAlgorithm { get; }

    public string GetName()
    {
        return Path.GetFileName(_path);
    }

    public string GetPath() => _path;

    public void Write(BackupObject backupObject, IRepository repository, string archiveName)
    {
        StorageAlgorithm.WriteStorageFile(backupObject, repository, archiveName);
    }
}