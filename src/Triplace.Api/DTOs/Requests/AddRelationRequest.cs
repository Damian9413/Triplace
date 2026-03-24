namespace Triplace.Api.DTOs.Requests;

public class AddRelationRequest
{
    public Guid AttractionIdA { get; set; }
    public Guid AttractionIdB { get; set; }
}

public class RemoveRelationRequest
{
    public Guid AttractionIdA { get; set; }
    public Guid AttractionIdB { get; set; }
    public string Type { get; set; } = string.Empty; // Exclusion, Recommendation
}
