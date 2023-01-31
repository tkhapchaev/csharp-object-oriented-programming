using Backups.Exceptions;
using Backups.Models;
using Zio;
using Zio.FileSystems;

namespace Backups.Entities;

public class InMemoryRepository : IRepository
{
    private readonly MemoryFileSystem _memoryFileSystem;

    private string _internalFolderName;

    public InMemoryRepository(MemoryFileSystem memoryFileSystem)
    {
        _memoryFileSystem = memoryFileSystem ?? throw new ArgumentNullException(nameof(memoryFileSystem));
        _internalFolderName = $"{UPath.DirectorySeparator}";
    }

    public string GetPath() => $"{_internalFolderName}";

    public string GetOriginalPath() => $"{UPath.DirectorySeparator}";

    public void SetInternalDirectory(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        _internalFolderName = $"{UPath.DirectorySeparator}{name}{UPath.DirectorySeparator}";
    }

    public char GetDirectorySeparatorChar()
    {
        return UPath.DirectorySeparator;
    }

    public Stream Append(string path)
    {
        return _memoryFileSystem.OpenFile(
            new UPath($"{_internalFolderName}{path}"),
            FileMode.Append,
            FileAccess.Write);
    }

    public Stream Write(string path)
    {
        return _memoryFileSystem.OpenFile(
            new UPath($"{_internalFolderName}{path}"),
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite);
    }

    public Stream WriteDirectly(string path)
    {
        return _memoryFileSystem.OpenFile(new UPath(path), FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public Stream Read(string path)
    {
        return _memoryFileSystem.OpenFile(new UPath(path), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
    }

    public IRepositoryObject GetRepositoryObject(BackupObject backupObject, IStorageAlgorithm storageAlgorithm)
    {
        if (_memoryFileSystem.DirectoryExists(backupObject.Path))
        {
            return new RepositoryFolder(backupObject.Path, storageAlgorithm);
        }

        if (_memoryFileSystem.FileExists(backupObject.Path))
        {
            return new RepositoryFile(backupObject.Path, storageAlgorithm);
        }

        throw RepositoryException.InvalidPath();
    }

    public void CreateDirectory(string path)
    {
        _memoryFileSystem.CreateDirectory(path);
    }

    public void DeleteDirectory(string path)
    {
        _memoryFileSystem.DeleteDirectory($"{GetPath()}{path}", true);
    }

    public List<IRepositoryObject> GetDirectoryFiles(string path, IStorageAlgorithm storageAlgorithm)
    {
        var files = _memoryFileSystem.EnumerateFiles(new UPath(path)).ToList();

        return new List<IRepositoryObject>(files.Select(file => new RepositoryFile(file.ToString(), storageAlgorithm))
            .ToList());
    }
}