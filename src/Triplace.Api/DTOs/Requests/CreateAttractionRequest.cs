namespace Triplace.Api.DTOs.Requests;

public class CreateAttractionRequest
{
    public string Name { get; set; } = string.Empty;
    public List<MetadataEntryRequest> Metadata { get; set; } = [];
}

public class MetadataEntryRequest
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
