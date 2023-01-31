using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities.Accounts;

public class DepositPercentage
{
    private readonly List<DepositRange> _depositRanges;

    public DepositPercentage()
    {
        _depositRanges = new List<DepositRange>();
    }

    public IReadOnlyCollection<DepositRange> DepositRanges => _depositRanges.AsReadOnly();

    public void AddDepositRange(DepositRange depositRange)
    {
        ArgumentNullException.ThrowIfNull(depositRange);
        _depositRanges.Add(depositRange);
    }

    public void RemoveDepositRange(DepositRange depositRange)
    {
        ArgumentNullException.ThrowIfNull(depositRange);
        _depositRanges.Remove(depositRange);
    }

    public double GetDepositPercentage(decimal money)
    {
        foreach (DepositRange depositRange in _depositRanges.Where(depositRange =>
                     money >= depositRange.RangeStart && money <= depositRange.RangeEnd))
        {
            return depositRange.Percent;
        }

        throw DepositPercentageException.NoAppropriateRange(money);
    }
}