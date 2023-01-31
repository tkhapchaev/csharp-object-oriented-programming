using System.IO.Compression;
using Backups.Entities;

namespace Backups.Models;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public List<IStorage> Store(RestorePoint restorePoint)
    {
        var singleStorage = new SingleStorage(restorePoint.CreationDate, new SingleStorageArchiver());

        foreach (BackupObject backupObject in restorePoint.BackupObjects)
        {
            singleStorage.AddBackupObject(backupObject);
        }

        return new List<IStorage> { singleStorage };
    }

    public void WriteStorageFile(BackupObject backupObject, IRepository repository, string archiveName)
    {
        var archive = new ZipArchive(repository.Write(archiveName), ZipArchiveMode.Update, false);
        using Stream source = repository.Read(backupObject.Path);
        ZipArchiveEntry entry = archive.CreateEntry(backupObject.GetName());
        source.CopyTo(entry.Open());
        archive.Dispose();
    }

    public void WriteStorageFolder(BackupObject backupObject, IRepository repository, string archiveName)
    {
        string folderArchiveName = $"{repository.GetPath()}{backupObject.GetName()}.zip",
            zipArchiveName = $"{backupObject.GetName()}.zip";

        using var folderArchive = new ZipArchive(repository.Write(zipArchiveName), ZipArchiveMode.Create, false);
        folderArchive.Dispose();

        foreach (IRepositoryObject repositoryObject in repository.GetDirectoryFiles(backupObject.Path, this))
        {
            using var zipArchive = new ZipArchive(repository.Write(zipArchiveName), ZipArchiveMode.Update, false);
            using Stream source = repository.Read(repositoryObject.GetPath());
            ZipArchiveEntry entry = zipArchive.CreateEntry(repositoryObject.GetName());
            source.CopyTo(entry.Open());
            zipArchive.Dispose();
        }

        using (var archive = new ZipArchive(repository.Write(archiveName), ZipArchiveMode.Update, false))
        {
            using Stream archiveSource = repository.Read(folderArchiveName);
            ZipArchiveEntry folderEntry = archive.CreateEntry(Path.GetFileName(folderArchiveName));
            archiveSource.CopyTo(folderEntry.Open());
            archive.Dispose();
        }

        File.Delete(folderArchiveName);
    }
}