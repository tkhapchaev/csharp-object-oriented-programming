using Backups.Entities;

namespace Backups.Extra.Models;

public interface IRestorer
{
    void RestoreToOriginalLocation(RestorePoint restorePoint);

    void RestoreToDifferentLocation(RestorePoint restorePoint);
}