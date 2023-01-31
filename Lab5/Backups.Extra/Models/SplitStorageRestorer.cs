using System.IO.Compression;
using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Models;

public class SplitStorageRestorer : IRestorer
{
    public SplitStorageRestorer(IRepository repositoryFrom, IRepository repositoryTo)
    {
        RepositoryFrom = repositoryFrom ?? throw new ArgumentNullException(nameof(repositoryFrom));
        RepositoryTo = repositoryTo ?? throw new ArgumentNullException(nameof(repositoryTo));
    }

    public IRepository RepositoryFrom { get; }

    public IRepository RepositoryTo { get; }

    public void RestoreToOriginalLocation(RestorePoint restorePoint)
    {
        var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);

        string restorePointPath =
            $"{RepositoryFrom.GetPath()}{restorePointDateTimeConverter.Convert()}{RepositoryFrom.GetDirectorySeparatorChar()}";

        List<IRepositoryObject> repositoryObjects =
            RepositoryFrom.GetDirectoryFiles(restorePointPath, new SplitStorageAlgorithm());

        foreach (string archiveName in repositoryObjects.Select(repositoryObject => repositoryObject.GetName()))
        {
            using var zipArchive = new ZipArchive(
                RepositoryFrom.Read($"{restorePointPath}{archiveName}"),
                ZipArchiveMode.Read);

            foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
            {
                foreach (BackupObject backupObject in restorePoint.BackupObjects)
                {
                    if (zipArchiveEntry.Name == backupObject.GetName())
                    {
                        int lastSeparatorCharPosition =
                            backupObject.Path.LastIndexOf(RepositoryFrom.GetDirectorySeparatorChar());

                        string fileName = backupObject.Path.Substring(lastSeparatorCharPosition + 1),
                            repositoryPath = backupObject.Path.Substring(0, lastSeparatorCharPosition + 1);

                        using Stream entry = zipArchiveEntry.Open();
                        using Stream fileToRestore = RepositoryTo.WriteDirectly($"{repositoryPath}{fileName}");

                        entry.CopyTo(fileToRestore);

                        break;
                    }
                }
            }
        }
    }

    public void RestoreToDifferentLocation(RestorePoint restorePoint)
    {
        var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);

        string restorePointPath =
            $"{RepositoryFrom.GetPath()}{restorePointDateTimeConverter.Convert()}{RepositoryFrom.GetDirectorySeparatorChar()}";

        List<IRepositoryObject> repositoryObjects =
            RepositoryFrom.GetDirectoryFiles(restorePointPath, new SplitStorageAlgorithm());

        foreach (string archiveName in repositoryObjects.Select(repositoryObject => repositoryObject.GetName()))
        {
            using var zipArchive = new ZipArchive(
                RepositoryFrom.Read($"{restorePointPath}{archiveName}"),
                ZipArchiveMode.Read);

            foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
            {
                using Stream entry = zipArchiveEntry.Open();
                using Stream fileToRestore = RepositoryTo.Write(zipArchiveEntry.Name);

                entry.CopyTo(fileToRestore);
            }
        }
    }
}