namespace Triplace.Api.DTOs.Responses;

public class SeasonalCatalogResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CatalogMetadataResponse Metadata { get; set; } = new();
    public List<CatalogEntryResponse> Entries { get; set; } = [];
}

public class CatalogMetadataResponse
{
    public string Season { get; set; } = string.Empty;
    public string ValidFrom { get; set; } = string.Empty;
    public string ValidTo { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? MaxCapacity { get; set; }
}

public class CatalogEntryResponse
{
    public Guid Id { get; set; }
    public Guid AttractionId { get; set; }
    public string SnapshotName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime PublishedAt { get; set; }
}
