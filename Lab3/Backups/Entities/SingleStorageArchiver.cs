using System.IO.Compression;
using Backups.Models;

namespace Backups.Entities;

public class SingleStorageArchiver : ISingleStorageArchiver
{
    public void Archive(SingleStorage storage, IRepository repository)
    {
        char directorySeparatorChar = repository.GetDirectorySeparatorChar();
        string storageCreationDate = $"{storage.GetCreationDate()}{directorySeparatorChar}";

        string archiveName = $"{storageCreationDate}{Guid.NewGuid().ToString()}.zip";
        using var archive = new ZipArchive(repository.Write(archiveName), ZipArchiveMode.Create, false);

        archive.Dispose();

        foreach (BackupObject backupObject in storage.BackupObjects)
        {
            IRepositoryObject repositoryObject =
                repository.GetRepositoryObject(backupObject, storage.GetCreationAlgorithm());

            repositoryObject.Write(backupObject, repository, archiveName);
        }
    }
}