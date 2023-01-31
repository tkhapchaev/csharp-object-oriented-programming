using Backups.Models;

namespace Backups.Entities;

public class SplitStorageArchiver : ISplitStorageArchiver
{
    public void Archive(SplitStorage storage, IRepository repository)
    {
        BackupObject backupObject = storage.BackupObject;
        IRepositoryObject repositoryObject =
            repository.GetRepositoryObject(backupObject, storage.GetCreationAlgorithm());

        char directorySeparatorChar = repository.GetDirectorySeparatorChar();
        string storageCreationDate = $"{storage.GetCreationDate()}{directorySeparatorChar}";

        string archiveName = $"{storageCreationDate}{backupObject.GetName()}.zip";

        repositoryObject.Write(backupObject, repository, archiveName);
    }
}