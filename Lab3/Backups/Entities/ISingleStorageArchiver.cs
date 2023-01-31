namespace Backups.Entities;

public interface ISingleStorageArchiver
{
    void Archive(SingleStorage storage, IRepository repository);
}