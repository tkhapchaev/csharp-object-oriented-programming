using Backups.Models;

namespace Backups.Entities;

public interface IRepository
{
    string GetPath();

    string GetOriginalPath();

    void SetInternalDirectory(string name);

    char GetDirectorySeparatorChar();

    Stream Append(string path);

    Stream Write(string path);

    Stream WriteDirectly(string path);

    Stream Read(string path);

    IRepositoryObject GetRepositoryObject(BackupObject backupObject, IStorageAlgorithm storageAlgorithm);

    void CreateDirectory(string path);

    void DeleteDirectory(string path);

    List<IRepositoryObject> GetDirectoryFiles(string path, IStorageAlgorithm storageAlgorithm);
}