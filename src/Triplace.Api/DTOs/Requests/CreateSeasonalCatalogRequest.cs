namespace Triplace.Api.DTOs.Requests;

public class CreateSeasonalCatalogRequest
{
    public string Name { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty; // Spring, Summer, Autumn, Winter
    public string ValidFrom { get; set; } = string.Empty; // yyyy-MM-dd
    public string ValidTo { get; set; } = string.Empty;   // yyyy-MM-dd
    public string Region { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? MaxCapacity { get; set; }
}
