using Backups.Models;

namespace Backups.Entities;

public interface IStorage
{
    string GetCreationDate();

    IStorageAlgorithm GetCreationAlgorithm();

    void Archive(IRepository repository);
}