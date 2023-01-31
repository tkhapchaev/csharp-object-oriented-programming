using System.Globalization;
using System.Text;
using Backups.Entities;

namespace Backups.Extra.Models;

public class RestorePointDateTimeConverter
{
    public RestorePointDateTimeConverter(RestorePoint restorePoint)
    {
        RestorePoint = restorePoint ?? throw new ArgumentNullException(nameof(restorePoint));
    }

    public RestorePoint RestorePoint { get; }

    public string Convert()
    {
        var creationDateString = new StringBuilder(RestorePoint.CreationDate.ToString(CultureInfo.InvariantCulture));
        creationDateString.Replace(" ", "_");
        creationDateString.Replace(":", "_");
        creationDateString.Replace("/", "-");

        return creationDateString.ToString();
    }
}