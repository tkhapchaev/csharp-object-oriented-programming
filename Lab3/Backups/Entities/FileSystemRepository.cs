using Backups.Exceptions;
using Backups.Models;

namespace Backups.Entities;

public class FileSystemRepository : IRepository
{
    private readonly string _path;

    private string _internalFolderName;

    public FileSystemRepository(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!path.EndsWith(Path.DirectorySeparatorChar))
        {
            throw RepositoryException.PathShouldEndWithDirectorySeparatorChar();
        }

        _path = path;
        _internalFolderName = string.Empty;
    }

    public string GetPath() => $"{_path}{_internalFolderName}";

    public string GetOriginalPath() => $"{_path}";

    public void SetInternalDirectory(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        _internalFolderName = $"{name}{Path.DirectorySeparatorChar}";
    }

    public char GetDirectorySeparatorChar()
    {
        return Path.DirectorySeparatorChar;
    }

    public Stream Append(string path)
    {
        return new FileStream(path: $"{_path}{_internalFolderName}{path}", FileMode.Append, FileAccess.Write);
    }

    public Stream Write(string path)
    {
        return new FileStream(path: $"{_path}{_internalFolderName}{path}", FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public Stream WriteDirectly(string path)
    {
        return new FileStream(path: path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public Stream Read(string path)
    {
        return File.OpenRead(path);
    }

    public IRepositoryObject GetRepositoryObject(BackupObject backupObject, IStorageAlgorithm storageAlgorithm)
    {
        if (Directory.Exists(backupObject.Path))
        {
            return new RepositoryFolder(backupObject.Path, storageAlgorithm);
        }

        if (File.Exists(backupObject.Path))
        {
            return new RepositoryFile(backupObject.Path, storageAlgorithm);
        }

        throw RepositoryException.InvalidPath();
    }

    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public void DeleteDirectory(string path)
    {
        Directory.Delete($"{GetPath()}{path}", true);
    }

    public List<IRepositoryObject> GetDirectoryFiles(string path, IStorageAlgorithm storageAlgorithm)
    {
        var files = Directory.GetFiles(path).ToList();

        return new List<IRepositoryObject>(files.Select(file => new RepositoryFile(file, storageAlgorithm)).ToList());
    }
}