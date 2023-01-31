using System.Text.Json.Serialization;
using Backups.Entities;

namespace Backups.Extra.Entities.CleaningAlgorithm;

public class DateTimeCleaner : ICleaningAlgorithm
{
    [JsonConstructor]
    public DateTimeCleaner(DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        DateTimeFrom = dateTimeFrom;
        DateTimeTo = dateTimeTo;
    }

    public DateTime DateTimeFrom { get; set; }

    public DateTime DateTimeTo { get; set; }

    public List<RestorePoint> Clean(List<RestorePoint> restorePoints)
    {
        return restorePoints.Where(restorePoint =>
            restorePoint.CreationDate < DateTimeFrom || restorePoint.CreationDate > DateTimeTo).ToList();
    }
}