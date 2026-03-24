using Triplace.Domain.Enums;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Builders;

public class CatalogMetadataBuilder
{
    private Season _season;
    private DateOnly _validFrom;
    private DateOnly _validTo;
    private string _region = string.Empty;
    private string _description = string.Empty;
    private int? _maxCapacity;

    public CatalogMetadataBuilder ForSeason(Season season) { _season = season; return this; }
    public CatalogMetadataBuilder ValidFrom(DateOnly from) { _validFrom = from; return this; }
    public CatalogMetadataBuilder ValidTo(DateOnly to) { _validTo = to; return this; }
    public CatalogMetadataBuilder InRegion(string region) { _region = region; return this; }
    public CatalogMetadataBuilder WithDescription(string description) { _description = description; return this; }
    public CatalogMetadataBuilder WithMaxCapacity(int capacity) { _maxCapacity = capacity; return this; }

    public CatalogMetadata Build()
        => new(_season, _validFrom, _validTo, _region, _description, _maxCapacity);
}
