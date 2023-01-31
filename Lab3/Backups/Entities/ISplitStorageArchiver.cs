namespace Backups.Entities;

public interface ISplitStorageArchiver
{
    void Archive(SplitStorage storage, IRepository repository);
}