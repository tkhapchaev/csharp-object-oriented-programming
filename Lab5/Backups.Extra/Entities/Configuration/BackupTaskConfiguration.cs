using System.Text.Json.Serialization;
using Backups.Extra.Entities.CleaningAlgorithm;
using Backups.Extra.Services;
using Backups.Models;

namespace Backups.Extra.Entities.Configuration;

public class BackupTaskConfiguration
{
    public BackupTaskConfiguration(BackupTaskExtended backupTask)
    {
        Name = backupTask.Name;
        StorageWriter = backupTask.GetStorageWriter().GetType().Name;
        StorageAlgorithm = backupTask.GetStorageAlgorithm().GetType().Name;
        Repository = backupTask.GetRepository().GetType().Name;
        Path = backupTask.GetRepository().GetOriginalPath();
        Logger = backupTask.GetLogger().GetType().Name;
        LoggerPath = backupTask.GetRepository().GetPath();
        LogFileName = $"{backupTask.Name}.log";
        LogWithTimeCode = backupTask.GetLogger().LogWithTimeCode;
        DateTimeCleaner = backupTask.GetDateTimeCleaner();
        AmountCleaner = backupTask.GetAmountCleaner();

        BackupObjects = new List<BackupObject>();

        foreach (BackupObject backupObject in backupTask.BackupObjects)
        {
            BackupObjects.Add(backupObject);
        }
    }

    [JsonConstructor]
    public BackupTaskConfiguration(
        string name,
        string storageWriter,
        string storageAlgorithm,
        string repository,
        string path,
        string logger,
        string loggerRepository,
        string loggerPath,
        string loggerFileName,
        bool logWithTimeCode,
        DateTimeCleaner dateTimeCleaner,
        AmountCleaner amountCleaner,
        List<BackupObject> backupObjects)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        StorageWriter = storageWriter ?? throw new ArgumentNullException(nameof(storageWriter));
        StorageAlgorithm = storageAlgorithm ?? throw new ArgumentNullException(nameof(storageAlgorithm));
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        LoggerPath = loggerPath ?? throw new ArgumentNullException(nameof(loggerPath));
        LogFileName = loggerFileName ?? throw new ArgumentNullException(nameof(loggerFileName));
        LogWithTimeCode = logWithTimeCode;
        DateTimeCleaner = dateTimeCleaner ?? throw new ArgumentNullException(nameof(dateTimeCleaner));
        AmountCleaner = amountCleaner ?? throw new ArgumentNullException(nameof(amountCleaner));
        BackupObjects = backupObjects ?? throw new ArgumentNullException(nameof(backupObjects));
    }

    public string Name { get; }

    public string StorageWriter { get; }

    public string StorageAlgorithm { get; }

    public string Repository { get; }

    public string Path { get; }

    public string Logger { get; }

    public string LoggerPath { get; }

    public string LogFileName { get; }

    public bool LogWithTimeCode { get; }

    public DateTimeCleaner DateTimeCleaner { get; }

    public AmountCleaner AmountCleaner { get; }

    public List<BackupObject> BackupObjects { get; }
}