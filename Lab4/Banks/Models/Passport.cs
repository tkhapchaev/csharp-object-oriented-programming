using Banks.Exceptions;

namespace Banks.Models;

public class Passport
{
    public Passport(int series, int number)
    {
        if (series <= 0)
        {
            throw PassportException.InvalidSeries(series);
        }

        if (number <= 0)
        {
            throw PassportException.InvalidNumber(number);
        }

        Series = series;
        Number = number;
    }

    public int Series { get; }

    public int Number { get; }

    public override string ToString()
    {
        return $"Series: {Series}; Number: {Number}.";
    }
}