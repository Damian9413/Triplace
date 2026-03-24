using Triplace.Domain.Entities;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Domain.Builders;

public class RouteBuilder
{
    private readonly string _name;
    private string _description = string.Empty;
    private Season _season;
    private AttractionGroupId? _scopeGroupId;
    private readonly List<(CatalogEntryId EntryId, Priority Priority)> _items = new();

    private RouteBuilder(string name) => _name = name;

    public static RouteBuilder Create(string name) => new(name);

    public RouteBuilder WithDescription(string description) { _description = description; return this; }
    public RouteBuilder ForSeason(Season season) { _season = season; return this; }
    public RouteBuilder ScopedTo(AttractionGroupId groupId) { _scopeGroupId = groupId; return this; }

    public RouteBuilder WithItem(CatalogEntryId catalogEntryId, Priority priority)
    {
        _items.Add((catalogEntryId, priority));
        return this;
    }

    public Route Build()
    {
        var route = Route.Create(_name, _description, _season, _scopeGroupId);
        foreach (var (entryId, priority) in _items)
            route.AddItem(entryId, priority);
        return route;
    }
}
