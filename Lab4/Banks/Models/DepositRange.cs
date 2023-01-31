namespace Banks.Models;

public class DepositRange
{
    public DepositRange(decimal rangeStart, decimal rangeEnd, double percent)
    {
        RangeStart = rangeStart;
        RangeEnd = rangeEnd;
        Percent = percent;
    }

    public decimal RangeStart { get; }

    public decimal RangeEnd { get; }

    public double Percent { get; }
}