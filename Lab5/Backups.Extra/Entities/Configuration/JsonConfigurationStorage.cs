using System.Text.Json;
using Backups.Entities;
using Backups.Extra.Entities.Logger;
using Backups.Extra.Exceptions;
using Backups.Extra.Services;
using Backups.Models;
using Zio.FileSystems;

namespace Backups.Extra.Entities.Configuration;

public class JsonConfigurationStorage : IConfigurationStorage
{
    public JsonConfigurationStorage()
    {
        ConfigurationPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}configuration.json";
    }

    public string ConfigurationPath { get; }

    public void Save(List<BackupTaskExtended> backupTasks)
    {
        var backupTaskConfigurations = backupTasks
            .Select(backupTaskExtended => new BackupTaskConfiguration(backupTaskExtended)).ToList();

        var serializedConfigurations = backupTaskConfigurations
            .Select(backupTaskConfiguration => JsonSerializer.Serialize(backupTaskConfiguration)).ToList();

        if (File.Exists(ConfigurationPath))
        {
            File.Delete(ConfigurationPath);
        }

        File.AppendAllLines(ConfigurationPath, serializedConfigurations);
    }

    public List<BackupTaskExtended> Load()
    {
        var backupTasks = new List<BackupTaskExtended>();

        var serializedConfigurations = File.ReadAllLines(ConfigurationPath).ToList();

        var deserializedConfigurations = serializedConfigurations.Select(serializedConfiguration =>
            JsonSerializer.Deserialize<BackupTaskConfiguration>(serializedConfiguration) ??
            throw JsonConfigurationException.CannotDeserializeBackupTaskConfiguration()).ToList();

        foreach (BackupTaskConfiguration backupTaskConfiguration in deserializedConfigurations)
        {
            IStorageWriter? storageWriter = null;
            IStorageAlgorithm? storageAlgorithm = null;
            IRepository? repository = null;
            ILogger? logger = null;

            if (backupTaskConfiguration.StorageWriter == "Writer")
            {
                storageWriter = new Writer();
            }

            if (backupTaskConfiguration.StorageAlgorithm == "SingleStorageAlgorithm")
            {
                storageAlgorithm = new SingleStorageAlgorithm();
            }

            if (backupTaskConfiguration.StorageAlgorithm == "SplitStorageAlgorithm")
            {
                storageAlgorithm = new SplitStorageAlgorithm();
            }

            if (backupTaskConfiguration.Repository == "FileSystemRepository")
            {
                repository = new FileSystemRepository(backupTaskConfiguration.Path);
            }

            if (backupTaskConfiguration.Repository == "InMemoryRepository")
            {
                repository = new InMemoryRepository(new MemoryFileSystem());
            }

            if (backupTaskConfiguration.Logger == "ConsoleLogger")
            {
                logger = new ConsoleLogger(backupTaskConfiguration.LogWithTimeCode);
            }

            if (backupTaskConfiguration.Logger == "FileLogger")
            {
                IRepository? loggerRepository = null;

                if (backupTaskConfiguration.Repository == "FileSystemRepository")
                {
                    loggerRepository = new FileSystemRepository(backupTaskConfiguration.LoggerPath);
                }

                if (backupTaskConfiguration.Repository == "InMemoryRepository")
                {
                    loggerRepository = new InMemoryRepository(new MemoryFileSystem());
                }

                if (loggerRepository is null)
                {
                    throw JsonConfigurationException.CannotDeserializeBackupTaskConfiguration();
                }

                logger = new FileLogger(
                    loggerRepository,
                    backupTaskConfiguration.LogFileName,
                    backupTaskConfiguration.LogWithTimeCode);
            }

            if (storageWriter is null || storageAlgorithm is null || repository is null || logger is null)
            {
                throw JsonConfigurationException.CannotDeserializeBackupTaskConfiguration();
            }

            var backupTask = new BackupTaskExtended(
                backupTaskConfiguration.Name,
                storageWriter,
                storageAlgorithm,
                repository,
                logger,
                backupTaskConfiguration.DateTimeCleaner,
                backupTaskConfiguration.AmountCleaner);

            foreach (BackupObject backupObject in backupTaskConfiguration.BackupObjects)
            {
                backupTask.AddBackupObject(backupObject);
            }

            backupTasks.Add(backupTask);
        }

        return backupTasks;
    }
}