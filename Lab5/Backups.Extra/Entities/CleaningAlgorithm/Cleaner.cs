using Backups.Entities;
using Backups.Extra.Models;

namespace Backups.Extra.Entities.CleaningAlgorithm;

public class Cleaner
{
    public Cleaner(
        Backup backup,
        IRepository repository,
        DateTimeCleaner dateTimeCleaner,
        AmountCleaner amountCleaner,
        bool useDateTimeCleaner,
        bool useAmountCleaner,
        bool intersectCleanersResult)
    {
        Backup = backup ?? throw new ArgumentNullException(nameof(backup));
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        DateTimeCleaner = dateTimeCleaner ?? throw new ArgumentNullException(nameof(dateTimeCleaner));
        AmountCleaner = amountCleaner ?? throw new ArgumentNullException(nameof(amountCleaner));

        UseDateTimeCleaner = useDateTimeCleaner;
        UseAmountCleaner = useAmountCleaner;
        IntersectCleanersResults = intersectCleanersResult;
    }

    public Backup Backup { get; }

    public IRepository Repository { get; }

    public DateTimeCleaner DateTimeCleaner { get; }

    public AmountCleaner AmountCleaner { get; }

    public bool IntersectCleanersResults { get; set; }

    public bool UseDateTimeCleaner { get; set; }

    public bool UseAmountCleaner { get; set; }

    public void Clean()
    {
        if (!UseDateTimeCleaner && !UseAmountCleaner)
        {
            return;
        }

        List<RestorePoint> toRemoveByAmount = new List<RestorePoint>(), toRemoveByDateTime = new List<RestorePoint>();

        if (!UseDateTimeCleaner && UseAmountCleaner)
        {
            toRemoveByAmount.AddRange(AmountCleaner.Clean(new List<RestorePoint>(Backup.RestorePoints)));

            foreach (RestorePoint restorePoint in toRemoveByAmount)
            {
                Backup.RemoveRestorePoint(restorePoint);
                var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);
                Repository.DeleteDirectory(restorePointDateTimeConverter.Convert());
            }

            return;
        }

        if (UseDateTimeCleaner && !UseAmountCleaner)
        {
            toRemoveByDateTime.AddRange(DateTimeCleaner.Clean(new List<RestorePoint>(Backup.RestorePoints)));

            foreach (RestorePoint restorePoint in toRemoveByDateTime)
            {
                Backup.RemoveRestorePoint(restorePoint);
                var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);
                Repository.DeleteDirectory(restorePointDateTimeConverter.Convert());
            }

            return;
        }

        if (UseDateTimeCleaner && UseAmountCleaner)
        {
            if (IntersectCleanersResults)
            {
                foreach (RestorePoint restorePoint in toRemoveByAmount.Where(restorePoint =>
                             toRemoveByDateTime.Contains(restorePoint)))
                {
                    Backup.RemoveRestorePoint(restorePoint);
                    var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);
                    Repository.DeleteDirectory(restorePointDateTimeConverter.Convert());
                }
            }
            else
            {
                foreach (RestorePoint restorePoint in toRemoveByAmount)
                {
                    Backup.RemoveRestorePoint(restorePoint);
                    var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);
                    Repository.DeleteDirectory(restorePointDateTimeConverter.Convert());
                }

                foreach (RestorePoint restorePoint in toRemoveByDateTime)
                {
                    Backup.RemoveRestorePoint(restorePoint);
                    var restorePointDateTimeConverter = new RestorePointDateTimeConverter(restorePoint);
                    Repository.DeleteDirectory(restorePointDateTimeConverter.Convert());
                }
            }
        }
    }
}