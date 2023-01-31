using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Stream
{
    private readonly List<OgnpGroup> _groups;

    public Stream(Ognp ognp, Teacher teacher)
    {
        Ognp = ognp ?? throw new ArgumentNullException();

        Schedule = new Schedule();

        Teacher = teacher ?? throw new ArgumentNullException();

        _groups = new List<OgnpGroup>();
    }

    public Ognp Ognp { get; }

    public Schedule Schedule { get; }

    public Teacher Teacher { get; }

    public IReadOnlyList<OgnpGroup> Groups => _groups.AsReadOnly();

    public void AddGroup(OgnpGroup ognpGroup)
    {
        if (_groups.Contains(ognpGroup))
        {
            throw StreamException.StreamAlreadyContainsSuchGroup(Ognp.Name);
        }

        _groups.Add(ognpGroup);
    }

    public void RemoveGroup(OgnpGroup ognpGroup)
    {
        if (!_groups.Contains(ognpGroup))
        {
            throw StreamException.StreamHasNoSuchGroup(Ognp.Name);
        }

        _groups.Remove(ognpGroup);
    }
}