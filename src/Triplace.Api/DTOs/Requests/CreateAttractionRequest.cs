namespace Triplace.Api.DTOs.Requests;

public class CreateAttractionRequest
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> BestSeasons { get; set; } = [];
    public string Duration { get; set; } = string.Empty;
    public bool IsOutdoor { get; set; }
    public bool IsFree { get; set; }
    public List<MetadataEntryRequest> Metadata { get; set; } = [];
}

public class MetadataEntryRequest
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
