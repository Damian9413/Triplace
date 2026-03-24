using Triplace.Domain.Ids;
using Triplace.Domain.Interfaces;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class AttractionEntry
{
    private readonly List<AttractionAddonInstance> _addons = new();

    public AttractionEntryId Id { get; }
    public IAttractionNode Node { get; }
    public IReadOnlyList<AttractionAddonInstance> Addons => _addons.AsReadOnly();

    internal AttractionEntry(AttractionEntryId id, IAttractionNode node)
    {
        Id = id;
        Node = node;
    }

    public void AddAddon(AttractionAddonType type, Dictionary<string, object> values)
    {
        type.Validate(values);
        _addons.Add(new AttractionAddonInstance(type, values));
    }
}
