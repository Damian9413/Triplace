namespace Triplace.Api.DTOs.Responses;

public class SeasonalCatalogResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CatalogMetadataResponse Metadata { get; set; } = new();
    public List<Guid> AttractionIds { get; set; } = [];
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
