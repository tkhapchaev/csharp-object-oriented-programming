using System.Globalization;
using System.Text;
using Backups.Models;

namespace Backups.Entities;

public class SingleStorage : IStorage
{
    private readonly List<BackupObject> _backupObjects;

    private readonly ISingleStorageArchiver _storageArchiver;

    private readonly string _creationDate;

    public SingleStorage(DateTime creationDate, ISingleStorageArchiver storageArchiver)
    {
        _storageArchiver = storageArchiver ?? throw new ArgumentNullException(nameof(storageArchiver));

        var creationDateString = new StringBuilder(creationDate.ToString(CultureInfo.InvariantCulture));
        creationDateString.Replace(" ", "_");
        creationDateString.Replace(":", "_");
        creationDateString.Replace("/", "-");

        _creationDate = creationDateString.ToString();

        _backupObjects = new List<BackupObject>();
    }

    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects.AsReadOnly();

    public string GetCreationDate() => _creationDate;

    public IStorageAlgorithm GetCreationAlgorithm() => new SingleStorageAlgorithm();

    public void AddBackupObject(BackupObject backupObject)
    {
        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        _backupObjects.Remove(backupObject);
    }

    public void Archive(IRepository repository)
    {
        _storageArchiver.Archive(this, repository);
    }
}