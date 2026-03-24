namespace Triplace.Domain.ValueObjects;

public class AttractionMetadata
{
    private readonly List<MetadataEntry> _entries;

    public IReadOnlyList<MetadataEntry> Entries => _entries.AsReadOnly();

    public AttractionMetadata(IEnumerable<MetadataEntry> entries)
    {
        _entries = entries.ToList();
    }
}
