namespace Backups.Entities;

public class Writer : IStorageWriter
{
    private string _storageCreationDate;

    public Writer()
    {
        _storageCreationDate = string.Empty;
    }

    public void Write(List<IStorage> storages, IRepository repository)
    {
        char directorySeparatorChar = repository.GetDirectorySeparatorChar();
        repository.CreateDirectory(repository.GetPath());

        foreach (IStorage storage in storages)
        {
            _storageCreationDate = $"{storage.GetCreationDate()}{directorySeparatorChar}";
            repository.CreateDirectory($"{repository.GetPath()}{_storageCreationDate}");
            storage.Archive(repository);
        }
    }
}