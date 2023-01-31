namespace Backups.Entities;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Remove(restorePoint);
    }
}