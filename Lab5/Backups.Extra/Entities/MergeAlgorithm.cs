using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Entities;

public class MergeAlgorithm
{
    public MergeAlgorithm(RestorePoint oldRestorePoint, RestorePoint newRestorePoint)
    {
        NewRestorePoint = newRestorePoint ?? throw new ArgumentNullException(nameof(newRestorePoint));
        OldRestorePoint = oldRestorePoint ?? throw new ArgumentNullException(nameof(oldRestorePoint));
    }

    public RestorePoint OldRestorePoint { get; }

    public RestorePoint NewRestorePoint { get; }

    public RestorePoint Merge()
    {
        var result = new RestorePoint(NewRestorePoint.CreationDate);

        foreach (BackupObject backupObject in OldRestorePoint.BackupObjects)
        {
            if (!NewRestorePoint.BackupObjects.Contains(backupObject))
            {
                result.AddBackupObject(backupObject);
            }
        }

        foreach (BackupObject backupObject in NewRestorePoint.BackupObjects)
        {
            result.AddBackupObject(backupObject);
        }

        return result;
    }
}