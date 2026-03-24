namespace Triplace.Api.DTOs.Responses;

public class AttractionRelationResponse
{
    public Guid LowId { get; set; }
    public Guid HighId { get; set; }
    public string Type { get; set; } = string.Empty;
}
