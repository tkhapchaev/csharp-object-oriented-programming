using Isu.Exceptions;

namespace Isu.Entities;

public class Student
{
    public Student(int id, string name, Group group)
    {
        ArgumentNullException.ThrowIfNull(name);

        var fullName = name.Split(" ").ToList();

        if (fullName.Count != 2)
        {
            throw StudentException.InvalidStudentName(name);
        }

        Name = fullName[0][0].ToString().ToUpper() + fullName[0].Substring(1);

        Surname = fullName[1][0].ToString().ToUpper() + fullName[1].Substring(1);

        if (id <= 0)
        {
            throw StudentException.InvalidStudentIdValue(id);
        }

        Group = group ?? throw new ArgumentNullException();
    }

    public string Name { get; }

    public string Surname { get; }

    public int Id { get; }

    public Group Group { get; set; }
}