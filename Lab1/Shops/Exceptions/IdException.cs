namespace Shops.Exceptions;

public class IdException : Exception
{
    private IdException(int id)
    {
        Id = id;
    }

    public int Id { get; }

    public static IdException InvalidIdValue(int id) => new IdException(id);
}