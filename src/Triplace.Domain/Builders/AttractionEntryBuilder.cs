using Triplace.Domain.Entities;

namespace Triplace.Domain.Builders;

public class AttractionEntryBuilder
{
    private readonly AttractionEntry _entry;

    internal AttractionEntryBuilder(AttractionEntry entry) => _entry = entry;

    public AttractionEntryBuilder AddAddon(AttractionAddonType type, Dictionary<string, object> values)
    {
        _entry.AddAddon(type, values);
        return this;
    }
}
