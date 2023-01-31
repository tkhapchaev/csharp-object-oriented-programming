namespace Backups.Entities;

public interface IStorageWriter
{
    void Write(List<IStorage> storages, IRepository repository);
}