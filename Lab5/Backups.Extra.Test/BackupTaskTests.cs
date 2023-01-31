using System.Globalization;
using System.Text;
using Backups.Entities;
using Backups.Extra.Entities.CleaningAlgorithm;
using Backups.Extra.Entities.Logger;
using Backups.Extra.Models;
using Backups.Extra.Services;
using Backups.Models;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Extra.Test;

public class BackupTaskTests
{
    [Fact]
    public void RunBackupTask_RestorePointWasCreated()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText("/test1.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");
        memoryFileSystem.CreateDirectory("/test2");
        memoryFileSystem.WriteAllText("/test2/file.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");

        var backupObject1 = new BackupObject("/test1.txt");
        var backupObject2 = new BackupObject("/test2");

        var memoryFileSystem2 = new MemoryFileSystem();

        var backupTaskExtended = new BackupTaskExtended(
            "Backup",
            new Writer(),
            new SplitStorageAlgorithm(),
            new InMemoryRepository(memoryFileSystem),
            new FileLogger(new InMemoryRepository(memoryFileSystem2), "/backup.log", true),
            new DateTimeCleaner(new DateTime(2020, 1, 1), new DateTime(2023, 1, 1)),
            new AmountCleaner(10));

        string creationDate =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

        backupTaskExtended.AddBackupObject(backupObject1);
        backupTaskExtended.AddBackupObject(backupObject2);

        backupTaskExtended.NewBackup();

        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate}/test1.txt.zip")));
        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate}/test2.zip")));
    }

    [Fact]
    public void RunBackupTaskWithFileLogger_LogFileWasCreated()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText("/test1.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");

        var backupObject1 = new BackupObject("/test1.txt");

        var memoryFileSystem2 = new MemoryFileSystem();

        var backupTaskExtended = new BackupTaskExtended(
            "Backup",
            new Writer(),
            new SplitStorageAlgorithm(),
            new InMemoryRepository(memoryFileSystem),
            new FileLogger(new InMemoryRepository(memoryFileSystem2), "/backup.log", true),
            new DateTimeCleaner(new DateTime(2020, 1, 1), new DateTime(2023, 1, 1)),
            new AmountCleaner(10));

        string creationDate =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

        backupTaskExtended.AddBackupObject(backupObject1);
        backupTaskExtended.NewBackup();

        Assert.True(memoryFileSystem2.FileExists(new UPath($"/backup.log")));
    }

    [Fact]
    public void RunBackupTaskWithAmountLimit_CleanerMergedRestorePoints()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText("/test1.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");
        memoryFileSystem.CreateDirectory("/test2");
        memoryFileSystem.WriteAllText("/test2/file.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");

        var backupObject1 = new BackupObject("/test1.txt");
        var backupObject2 = new BackupObject("/test2");

        var memoryFileSystem2 = new MemoryFileSystem();

        var backupTaskExtended = new BackupTaskExtended(
            "Backup",
            new Writer(),
            new SplitStorageAlgorithm(),
            new InMemoryRepository(memoryFileSystem),
            new FileLogger(new InMemoryRepository(memoryFileSystem2), "/backup.log", true),
            new DateTimeCleaner(new DateTime(2020, 1, 1), new DateTime(2023, 1, 1)),
            new AmountCleaner(1));

        backupTaskExtended.AddBackupObject(backupObject1);
        backupTaskExtended.AddBackupObject(backupObject2);
        backupTaskExtended.UseAmountCleaner = true;

        DateTime creationDate = DateTime.Now;

        backupTaskExtended.NewBackup();

        string creationDate2 =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(creationDate.ToString(CultureInfo.InvariantCulture)));

        creationDate = creationDate.AddSeconds(-2);

        string creationDate3 =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(creationDate.ToString(CultureInfo.InvariantCulture)));

        memoryFileSystem.MoveDirectory(new UPath($"/Backup/{creationDate2}/"), new UPath($"/Backup/{creationDate3}/"));

        backupTaskExtended.NewBackup();

        Assert.False(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate2}/test1.txt.zip")));
        Assert.False(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate2}/test2.zip")));
        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate3}/test1.txt.zip")));
        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate3}/test2.zip")));
    }

    [Fact]
    public void RestoreBackupObjectsToOriginalLocation_BackupObjectsWereRestored()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText("/test1.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");

        var backupObject1 = new BackupObject("/test1.txt");
        var repository = new InMemoryRepository(memoryFileSystem);

        var memoryFileSystem2 = new MemoryFileSystem();

        var backupTaskExtended = new BackupTaskExtended(
            "Backup",
            new Writer(),
            new SplitStorageAlgorithm(),
            repository,
            new FileLogger(new InMemoryRepository(memoryFileSystem2), "/backup.log", true),
            new DateTimeCleaner(new DateTime(2020, 1, 1), new DateTime(2023, 1, 1)),
            new AmountCleaner(10));

        string creationDate =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

        backupTaskExtended.AddBackupObject(backupObject1);
        backupTaskExtended.NewBackup();

        memoryFileSystem.DeleteFile(new UPath("/test1.txt"));

        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate}/test1.txt.zip")));

        var splitStorageRestorer = new SplitStorageRestorer(repository, repository);
        splitStorageRestorer.RestoreToOriginalLocation(backupTaskExtended.GetBackup().RestorePoints.First());

        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate}/test1.txt.zip")));
    }

    [Fact]
    public void RestoreBackupObjectsToDifferentLocation_BackupObjectsWereRestored()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText("/test1.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");
        memoryFileSystem.CreateDirectory("/test2");
        memoryFileSystem.WriteAllText("/test2/file.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");

        var backupObject1 = new BackupObject("/test1.txt");
        var backupObject2 = new BackupObject("/test2");

        var memoryFileSystem2 = new MemoryFileSystem();
        var repository = new InMemoryRepository(memoryFileSystem);

        var backupTaskExtended = new BackupTaskExtended(
            "Backup",
            new Writer(),
            new SplitStorageAlgorithm(),
            repository,
            new FileLogger(new InMemoryRepository(memoryFileSystem2), "/backup.log", true),
            new DateTimeCleaner(new DateTime(2020, 1, 1), new DateTime(2023, 1, 1)),
            new AmountCleaner(10));

        backupTaskExtended.AddBackupObject(backupObject1);

        string creationDate =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

        backupTaskExtended.NewBackup();

        Assert.True(memoryFileSystem.FileExists(new UPath($"/Backup/{creationDate}/test1.txt.zip")));

        var splitStorageRestorer = new SingleStorageRestorer(repository, new InMemoryRepository(memoryFileSystem2));
        splitStorageRestorer.RestoreToDifferentLocation(backupTaskExtended.GetBackup().RestorePoints.First());

        Assert.True(memoryFileSystem2.FileExists(new UPath("/test1.txt")));
    }

    private string ReplaceInvalidCharsInCreationDate(StringBuilder creationDate)
    {
        creationDate.Replace(" ", "_");
        creationDate.Replace(":", "_");
        creationDate.Replace("/", "-");

        return creationDate.ToString();
    }
}