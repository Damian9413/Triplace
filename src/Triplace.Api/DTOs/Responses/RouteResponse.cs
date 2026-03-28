namespace Triplace.Api.DTOs.Responses;

public class RouteResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public Guid? ScopeGroupId { get; set; }
    public List<RouteItemResponse> MustHaves { get; set; } = [];
    public List<RouteItemResponse> Optionals { get; set; } = [];
}

public class RouteItemResponse
{
    public Guid Id { get; set; }
    public Guid AttractionId { get; set; }
    public string Priority { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
