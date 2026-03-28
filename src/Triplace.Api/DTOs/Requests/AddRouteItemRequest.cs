namespace Triplace.Api.DTOs.Requests;

public class AddRouteItemRequest
{
    public Guid AttractionId { get; set; }
    public string Priority { get; set; } = "Must";
}
