namespace Triplace.Api.DTOs.Requests;

public class CreateRouteRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public Guid? ScopeGroupId { get; set; }
    public List<RouteItemRequest> Items { get; set; } = [];
}

public class RouteItemRequest
{
    public Guid AttractionId { get; set; }
    public string Priority { get; set; } = "Must";
}
