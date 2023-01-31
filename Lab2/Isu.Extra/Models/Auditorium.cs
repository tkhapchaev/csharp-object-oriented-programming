using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Auditorium
{
    public Auditorium(int number)
    {
        if (number <= 99)
        {
            throw AuditoriumException.InvalidAuditoriumNumber(number);
        }

        Number = number;
    }

    public int Number { get; }
}