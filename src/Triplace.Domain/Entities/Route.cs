namespace Triplace.Domain.Entities;

public class Route
{
    public Guid Id { get; init; }
    public string City { get; init; } = string.Empty;
    public List<RouteItem> Items { get; init; } = [];
}
