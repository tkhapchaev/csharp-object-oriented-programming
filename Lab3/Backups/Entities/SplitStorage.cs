using System.Globalization;
using System.Text;
using Backups.Models;

namespace Backups.Entities;

public class SplitStorage : IStorage
{
    private readonly ISplitStorageArchiver _storageArchiver;

    private readonly string _creationDate;

    public SplitStorage(BackupObject backupObject, DateTime creationDate, ISplitStorageArchiver storageArchiver)
    {
        _storageArchiver = storageArchiver ?? throw new ArgumentNullException(nameof(storageArchiver));
        BackupObject = backupObject ?? throw new ArgumentNullException(nameof(backupObject));

        var creationDateString = new StringBuilder(creationDate.ToString(CultureInfo.InvariantCulture));
        creationDateString.Replace(" ", "_");
        creationDateString.Replace(":", "_");
        creationDateString.Replace("/", "-");

        _creationDate = creationDateString.ToString();
    }

    public BackupObject BackupObject { get; }

    public string GetCreationDate() => _creationDate;

    public IStorageAlgorithm GetCreationAlgorithm() => new SplitStorageAlgorithm();

    public void Archive(IRepository repository)
    {
        _storageArchiver.Archive(this, repository);
    }
}