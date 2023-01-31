using System.Globalization;
using System.Text;
using Backups.Entities;
using Backups.Models;
using Backups.Services;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupTaskTests
{
    [Fact]
    public void RunTwoBackupTasks_TwoRestorePointsAndThreeStoragesHaveBeenCreated()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText("/test1.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");
        memoryFileSystem.CreateDirectory("/test2");
        memoryFileSystem.WriteAllText("/test2/file.txt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit...");

        var backupObject1 = new BackupObject("/test1.txt");
        var backupObject2 = new BackupObject("/test2");

        var backupTask = new BackupTask(
            "backup",
            new Writer(),
            new SplitStorageAlgorithm(),
            new InMemoryRepository(memoryFileSystem));

        string creationDate =
            ReplaceInvalidCharsInCreationDate(new StringBuilder(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

        backupTask.AddBackupObject(backupObject1);
        backupTask.AddBackupObject(backupObject2);

        backupTask.NewBackup();

        Assert.True(memoryFileSystem.FileExists(new UPath($"/backup/{creationDate}/test1.txt.zip")));
        Assert.True(memoryFileSystem.FileExists(new UPath($"/backup/{creationDate}/test2.zip")));
    }

    private string ReplaceInvalidCharsInCreationDate(StringBuilder creationDate)
    {
        creationDate.Replace(" ", "_");
        creationDate.Replace(":", "_");
        creationDate.Replace("/", "-");

        return creationDate.ToString();
    }
}