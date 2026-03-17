namespace Triplace.Domain.Entities;

public class RouteDraft
{
    public Guid Id { get; init; }
    public string City { get; init; } = string.Empty;
    public List<string> Preferences { get; init; } = [];
    public List<RouteItem> Items { get; set; } = [];
}
