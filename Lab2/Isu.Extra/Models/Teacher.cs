using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Teacher
{
    public Teacher(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var fullName = name.Split(" ").ToList();

        if (fullName.Count != 2)
        {
            throw TeacherException.InvalidTeacherName(name);
        }

        Name = fullName[0][0].ToString().ToUpper() + fullName[0].Substring(1);

        Surname = fullName[1][0].ToString().ToUpper() + fullName[1].Substring(1);
    }

    public string Name { get; }

    public string Surname { get; }
}