using Isu.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class MainGroup
{
    public MainGroup(Group group, Schedule schedule)
    {
        Group = group ?? throw new ArgumentNullException();

        Schedule = schedule ?? throw new ArgumentNullException();
    }

    public Group Group { get; }

    public Schedule Schedule { get; }
}