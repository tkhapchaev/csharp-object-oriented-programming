using Backups.Entities;
using Backups.Extra.Entities.CleaningAlgorithm;
using Backups.Extra.Entities.Logger;
using Backups.Models;

namespace Backups.Extra.Services;

public class BackupTaskExtended : IBackupTaskExtended
{
    private readonly List<BackupObject> _backupObjects;

    private IStorageWriter _storageWriter;

    private IStorageAlgorithm _storageAlgorithm;

    private IRepository _repository;

    private ILogger _logger;

    private DateTimeCleaner _dateTimeCleaner;

    private AmountCleaner _amountCleaner;

    private Backup _backup;

    public BackupTaskExtended(
        string name,
        IStorageWriter storageWriter,
        IStorageAlgorithm storageAlgorithm,
        IRepository repository,
        ILogger logger,
        DateTimeCleaner dateTimeCleaner,
        AmountCleaner amountCleaner)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _storageWriter = storageWriter ?? throw new ArgumentNullException(nameof(storageWriter));
        _storageAlgorithm = storageAlgorithm ?? throw new ArgumentNullException(nameof(storageAlgorithm));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dateTimeCleaner = dateTimeCleaner ?? throw new ArgumentNullException(nameof(dateTimeCleaner));
        _amountCleaner = amountCleaner ?? throw new ArgumentNullException(nameof(amountCleaner));

        IntersectCleanersResults = false;
        UseDateTimeCleaner = false;
        UseAmountCleaner = false;

        _repository.SetInternalDirectory(name);

        _backup = new Backup();
        _backupObjects = new List<BackupObject>();

        _logger.Log($"(!) Created backup task called \"{Name}\"\n");
    }

    public string Name { get; }

    public bool IntersectCleanersResults { get; set; }

    public bool UseDateTimeCleaner { get; set; }

    public bool UseAmountCleaner { get; set; }

    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects.AsReadOnly();

    public IStorageWriter GetStorageWriter() => _storageWriter;

    public IStorageAlgorithm GetStorageAlgorithm() => _storageAlgorithm;

    public IRepository GetRepository() => _repository;

    public ILogger GetLogger() => _logger;

    public Backup GetBackup() => _backup;

    public void AddBackupObject(BackupObject backupObject)
    {
        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        _backupObjects.Remove(backupObject);
    }

    public void NewBackup()
    {
        var restorePoint = new RestorePoint(DateTime.Now);

        foreach (BackupObject backupObject in _backupObjects)
        {
            restorePoint.AddBackupObject(backupObject);
        }

        _backup.AddRestorePoint(restorePoint);

        string backupObjects = "\n";

        foreach (BackupObject backupObject in restorePoint.BackupObjects)
        {
            backupObjects += $"{backupObject.Path}\n";
        }

        _logger.Log($"Created restore point with {restorePoint.BackupObjects.Count} backup objects: {backupObjects}");

        List<IStorage> storages = _storageAlgorithm.Store(restorePoint);

        string storageNames = "\n";

        foreach (IStorage storage in storages)
        {
            storageNames += $"{storage.GetType().Name}\n";
        }

        _logger.Log($"Created storages: {storageNames}");

        _storageWriter.Write(storages, _repository);

        _logger.Log($"Wrote storages via {_storageAlgorithm.GetType().Name} to {_repository.GetPath()}\n");

        Clean();
    }

    public void Clean()
    {
        var cleaner = new Cleaner(
            _backup,
            _repository,
            _dateTimeCleaner,
            _amountCleaner,
            UseDateTimeCleaner,
            UseAmountCleaner,
            IntersectCleanersResults);

        cleaner.Clean();
    }

    public DateTimeCleaner GetDateTimeCleaner() => _dateTimeCleaner;

    public AmountCleaner GetAmountCleaner() => _amountCleaner;
}