namespace Business.Layer.Entities.Employee;

public class Employee : IEmployee
{
    private readonly List<Employee> _subordinates;

    private readonly List<string> _permissions;

    public Employee(string name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _subordinates = new List<Employee>();
        _permissions = new List<string>();
    }

    public Guid Id { get; }

    public string Name { get; }

    public IReadOnlyList<Employee> Subordinates => _subordinates.AsReadOnly();

    public IReadOnlyList<string> Permissions => _permissions.AsReadOnly();

    public void AddSubordinate(Employee subordinate)
    {
        _subordinates.Add(subordinate);
    }

    public void RemoveSubordinate(Employee subordinate)
    {
        _subordinates.Remove(subordinate);
    }

    public void AddPermission(string messageType)
    {
        _permissions.Add(messageType);
    }
}