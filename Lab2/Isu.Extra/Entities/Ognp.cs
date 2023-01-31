using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Ognp
{
    private readonly List<Stream> _streams;

    public Ognp(string name, FacultyName faculty)
    {
        Name = name ?? throw new ArgumentNullException();

        Faculty = faculty;

        _streams = new List<Stream>();
    }

    public string Name { get; }

    public FacultyName Faculty { get; }

    public IReadOnlyList<Stream> Streams => _streams.AsReadOnly();

    public void AddStream(Stream stream)
    {
        if (_streams.Contains(stream))
        {
            throw OgnpException.OgnpAlreadyContainsSuchStream(Name);
        }

        _streams.Add(stream);
    }

    public void RemoveStream(Stream stream)
    {
        if (!_streams.Contains(stream))
        {
            throw OgnpException.OgnpHasNoSuchStream(Name);
        }

        _streams.Remove(stream);
    }
}