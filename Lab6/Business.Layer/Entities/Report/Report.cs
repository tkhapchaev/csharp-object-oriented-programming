using Business.Layer.Models.Message;

namespace Business.Layer.Entities.Report;

public class Report : IReport
{
    public Report()
    {
        Contents = string.Empty;
    }

    public string Contents { get; private set; }

    public string MakeReport(List<IMessage> messages)
    {
        Contents = string.Empty;

        Contents += $"{messages.Count} messages:{Environment.NewLine}{Environment.NewLine}";

        foreach (IMessage message in messages)
        {
            Contents += $"{message.ToString()}{Environment.NewLine}";
        }

        return Contents;
    }
}