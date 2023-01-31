using Shops.Exceptions;

namespace Shops.Entities;

public class Product
{
    public Product(string name, int id)
    {
        Name = name ?? throw new ArgumentNullException();

        if (id < 0)
        {
            throw IdException.InvalidIdValue(id);
        }

        Id = id;
    }

    public string Name { get; }

    public int Id { get; }
}