using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Builders;

public class AttractionMetadataBuilder
{
    private readonly List<MetadataEntry> _entries = new();

    public AttractionMetadataBuilder AddEntry(string label, string value)
    {
        _entries.Add(new MetadataEntry(label, value));
        return this;
    }

    public AttractionMetadata Build() => new(_entries);
}
