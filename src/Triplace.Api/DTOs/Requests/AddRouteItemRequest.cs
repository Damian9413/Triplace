namespace Triplace.Api.DTOs.Requests;

public class AddRouteItemRequest
{
    public Guid CatalogEntryId { get; set; }
    public string Priority { get; set; } = "Must";
}
