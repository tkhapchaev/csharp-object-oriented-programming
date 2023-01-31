using Backups.Extra.Services;

namespace Backups.Extra.Entities.Configuration;

public interface IConfigurationStorage
{
    void Save(List<BackupTaskExtended> backupTasks);

    List<BackupTaskExtended> Load();
}