using Backups.Entities;
using Backups.Extra.Entities.CleaningAlgorithm;
using Backups.Extra.Entities.Logger;
using Backups.Models;
using Backups.Services;

namespace Backups.Extra.Services;

public interface IBackupTaskExtended : IBackupTask
{
    IStorageWriter GetStorageWriter();

    IStorageAlgorithm GetStorageAlgorithm();

    IRepository GetRepository();

    ILogger GetLogger();

    Backup GetBackup();

    void Clean();

    DateTimeCleaner GetDateTimeCleaner();

    AmountCleaner GetAmountCleaner();
}