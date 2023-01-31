using System.Text.Json.Serialization;
using Backups.Entities;
using Backups.Extra.Exceptions;

namespace Backups.Extra.Entities.CleaningAlgorithm;

public class AmountCleaner : ICleaningAlgorithm
{
    private int _amount;

    [JsonConstructor]
    public AmountCleaner(int amount)
    {
        if (amount < 1)
        {
            throw CleaningAlgorithmException.InvalidLimit(amount);
        }

        _amount = amount;
    }

    public int Amount
    {
        get => _amount;

        set
        {
            if (value < 1)
            {
                throw CleaningAlgorithmException.InvalidLimit(value);
            }

            _amount = value;
        }
    }

    public List<RestorePoint> Clean(List<RestorePoint> restorePoints)
    {
        var result = new List<RestorePoint>();
        int restorePointCounter = 0;

        for (int i = restorePoints.Count - 1; i >= 0; i--)
        {
            if (restorePointCounter < _amount)
            {
                restorePointCounter++;
                continue;
            }

            restorePointCounter++;
            result.Add(restorePoints[i]);
        }

        return result;
    }
}