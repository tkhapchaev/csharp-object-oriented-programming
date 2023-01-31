using Backups.Entities;

namespace Backups.Extra.Entities.CleaningAlgorithm;

public interface ICleaningAlgorithm
{
    List<RestorePoint> Clean(List<RestorePoint> restorePoints);
}