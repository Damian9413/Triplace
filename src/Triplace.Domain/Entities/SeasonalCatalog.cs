using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class SeasonalCatalog
{
    private readonly List<CatalogEntry> _entries = new();

    public SeasonalCatalogId Id { get; }
    public string Name { get; }
    public CatalogMetadata Metadata { get; }
    public IReadOnlyList<CatalogEntry> Entries => _entries.AsReadOnly();

    private SeasonalCatalog(SeasonalCatalogId id, string name, CatalogMetadata metadata)
    {
        Id = id;
        Name = name;
        Metadata = metadata;
    }

    internal static SeasonalCatalog Create(string name, CatalogMetadata metadata)
        => new(SeasonalCatalogId.New(), name, metadata);

    public void Publish(Attraction attraction)
    {
        if (attraction.Status != AttractionStatus.Published)
            throw new DomainException(
                $"Cannot add attraction '{attraction.Name}' to catalog: it must be Published.");

        var entry = new CatalogEntry(CatalogEntryId.New(), attraction.Id, attraction.Name, DateTime.UtcNow);
        _entries.Add(entry);
    }

    public void Deactivate(CatalogEntryId id)
    {
        var entry = _entries.FirstOrDefault(e => e.Id == id)
            ?? throw new EntityNotFoundException($"CatalogEntry {id.Value} not found.");
        entry.Deactivate();
    }

    public bool IsAvailable(AttractionId id)
        => _entries.Any(e => e.AttractionId == id && e.IsActive);
}
