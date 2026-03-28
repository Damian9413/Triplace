using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class SeasonalCatalog
{
    private readonly List<AttractionId> _attractions = new();

    public SeasonalCatalogId Id { get; }
    public string Name { get; }
    public CatalogMetadata Metadata { get; }
    public IReadOnlyList<AttractionId> Attractions => _attractions.AsReadOnly();

    private SeasonalCatalog(SeasonalCatalogId id, string name, CatalogMetadata metadata)
    {
        Id = id;
        Name = name;
        Metadata = metadata;
    }

    internal static SeasonalCatalog Create(string name, CatalogMetadata metadata)
        => new(SeasonalCatalogId.New(), name, metadata);

    public void AddAttraction(Attraction attraction)
    {
        if (attraction.Status != AttractionStatus.Published)
            throw new DomainException(
                $"Cannot add attraction '{attraction.Name}' to catalog: it must be Published.");

        if (_attractions.Contains(attraction.Id))
            throw new DomainException(
                $"Attraction '{attraction.Name}' is already in this catalog.");

        _attractions.Add(attraction.Id);
    }

    public void RemoveAttraction(AttractionId id)
    {
        if (!_attractions.Remove(id))
            throw new EntityNotFoundException($"Attraction {id.Value} not found in catalog.");
    }

    public bool Contains(AttractionId id) => _attractions.Contains(id);
}
