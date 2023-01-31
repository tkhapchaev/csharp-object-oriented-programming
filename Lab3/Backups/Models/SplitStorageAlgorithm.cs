using System.IO.Compression;
using Backups.Entities;

namespace Backups.Models;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public List<IStorage> Store(RestorePoint restorePoint)
    {
        return new List<IStorage>(restorePoint.BackupObjects
            .Select(backupObject =>
                new SplitStorage(backupObject, restorePoint.CreationDate, new SplitStorageArchiver())).ToList());
    }

    public void WriteStorageFile(BackupObject backupObject, IRepository repository, string archiveName)
    {
        using var archive = new ZipArchive(repository.Write(archiveName), ZipArchiveMode.Create, false);
        using Stream source = repository.Read(backupObject.Path);
        ZipArchiveEntry entry = archive.CreateEntry(backupObject.GetName());
        source.CopyTo(entry.Open());
        archive.Dispose();
    }

    public void WriteStorageFolder(BackupObject backupObject, IRepository repository, string archiveName)
    {
        using var folderArchive = new ZipArchive(repository.Write(archiveName), ZipArchiveMode.Create, false);
        folderArchive.Dispose();

        foreach (IRepositoryObject repositoryObject in repository.GetDirectoryFiles(backupObject.Path, this))
        {
            using var archive = new ZipArchive(repository.Write(archiveName), ZipArchiveMode.Update, false);
            using Stream source = repository.Read(repositoryObject.GetPath());
            ZipArchiveEntry entry = archive.CreateEntry(repositoryObject.GetName());
            source.CopyTo(entry.Open());
            archive.Dispose();
        }
    }
}