namespace Triplace.Domain.Entities;

public class RouteItem
{
    public Guid AttractionId { get; init; }
    public int Order { get; init; }
    public RouteItemPriority Priority { get; init; }
}
