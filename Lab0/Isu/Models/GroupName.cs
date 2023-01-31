using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    public GroupName(string groupName)
    {
        if (groupName is null)
        {
            throw new ArgumentNullException();
        }

        const int minCourseValue = 1, maxCourseValue = 4, minGroupNameLength = 5, maxGroupNameLength = 6;

        if (!int.TryParse(groupName[2].ToString(), out int course))
        {
            throw GroupNameException.InvalidGroupName(groupName);
        }

        if ((groupName.Length != minGroupNameLength && groupName.Length != maxGroupNameLength) ||
            course is < minCourseValue or > maxCourseValue)
        {
            throw GroupNameException.InvalidGroupName(groupName);
        }

        Name = groupName;
    }

    public string Name { get; }
}