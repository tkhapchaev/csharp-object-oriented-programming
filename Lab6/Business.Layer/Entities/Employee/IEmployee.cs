namespace Business.Layer.Entities.Employee;

public interface IEmployee
{
    Guid Id { get; }

    string Name { get; }
}