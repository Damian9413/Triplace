using Triplace.Domain.Entities;
using Triplace.Domain.Interfaces;

namespace Triplace.Domain.Builders;

public class AttractionGroupBuilder
{
    private readonly string _name;
    private readonly List<(IAttractionNode Node, Action<AttractionEntryBuilder>? Configure)> _entries = new();

    private AttractionGroupBuilder(string name) => _name = name;

    public static AttractionGroupBuilder Create(string name) => new(name);

    public AttractionGroupBuilder WithEntry(IAttractionNode node)
    {
        _entries.Add((node, null));
        return this;
    }

    public AttractionGroupBuilder WithEntryAndAddons(IAttractionNode node, Action<AttractionEntryBuilder> configure)
    {
        _entries.Add((node, configure));
        return this;
    }

    public AttractionGroup Build()
    {
        var group = AttractionGroup.Create(_name);
        foreach (var (node, configure) in _entries)
        {
            var entry = group.AddEntry(node);
            if (configure != null)
            {
                var builder = new AttractionEntryBuilder(entry);
                configure(builder);
            }
        }
        return group;
    }
}
