using Triplace.Domain.Enums;

namespace Triplace.Domain.ValueObjects;

public class CatalogMetadata
{
    public Season Season { get; }
    public DateOnly ValidFrom { get; }
    public DateOnly ValidTo { get; }
    public string Region { get; }
    public string Description { get; }
    public int? MaxCapacity { get; }

    public CatalogMetadata(Season season, DateOnly validFrom, DateOnly validTo,
        string region, string description, int? maxCapacity = null)
    {
        Season = season;
        ValidFrom = validFrom;
        ValidTo = validTo;
        Region = region;
        Description = description;
        MaxCapacity = maxCapacity;
    }
}
