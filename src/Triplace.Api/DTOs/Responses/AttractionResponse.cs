namespace Triplace.Api.DTOs.Responses;

public class AttractionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public AttractionMetadataResponse Metadata { get; set; } = new();
}

public class AttractionMetadataResponse
{
    public List<MetadataEntryResponse> Entries { get; set; } = [];
}

public class MetadataEntryResponse
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
