using Triplace.Domain.Ids;
using Triplace.Domain.Interfaces;

namespace Triplace.Domain.Entities;

public class AttractionGroup : IAttractionNode
{
    private readonly List<AttractionEntry> _entries = new();

    public AttractionGroupId Id { get; }
    public string Name { get; }
    public IReadOnlyList<AttractionEntry> Entries => _entries.AsReadOnly();

    private AttractionGroup(AttractionGroupId id, string name)
    {
        Id = id;
        Name = name;
    }

    internal static AttractionGroup Create(string name)
        => new(AttractionGroupId.New(), name);

    public AttractionEntry AddEntry(IAttractionNode node)
    {
        var entry = new AttractionEntry(AttractionEntryId.New(), node);
        _entries.Add(entry);
        return entry;
    }

    public IReadOnlyList<Attraction> Flatten()
    {
        var result = new List<Attraction>();
        foreach (var entry in _entries)
            result.AddRange(entry.Node.Flatten());
        return result.AsReadOnly();
    }
}
