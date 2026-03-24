using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;

namespace Triplace.Domain.Entities;

public class Route
{
    private readonly List<RouteItem> _items = new();

    public RouteId Id { get; }
    public string Name { get; }
    public string Description { get; }
    public Season Season { get; }
    public AttractionGroupId? ScopeGroupId { get; }
    public IReadOnlyList<RouteItem> Items => _items.AsReadOnly();

    private Route(RouteId id, string name, string description, Season season, AttractionGroupId? scopeGroupId)
    {
        Id = id;
        Name = name;
        Description = description;
        Season = season;
        ScopeGroupId = scopeGroupId;
    }

    internal static Route Create(string name, string description, Season season, AttractionGroupId? scopeGroupId)
        => new(RouteId.New(), name, description, season, scopeGroupId);

    public void AddItem(CatalogEntryId catalogEntryId, Priority priority)
    {
        var sortOrder = _items.Count > 0 ? _items.Max(i => i.SortOrder) + 1 : 0;
        _items.Add(new RouteItem(RouteItemId.New(), catalogEntryId, priority, sortOrder));
    }

    public void RemoveItem(RouteItemId id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id)
            ?? throw new EntityNotFoundException($"RouteItem {id.Value} not found.");
        _items.Remove(item);
        RenumberItems();
    }

    public void Reorder(RouteItemId id, int newSortOrder)
    {
        var item = _items.FirstOrDefault(i => i.Id == id)
            ?? throw new EntityNotFoundException($"RouteItem {id.Value} not found.");

        newSortOrder = Math.Max(0, Math.Min(newSortOrder, _items.Count - 1));
        _items.Remove(item);
        _items.Insert(newSortOrder, item);
        RenumberItems();
    }

    public IReadOnlyList<RouteItem> GetMustHaves()
        => _items.Where(i => i.Priority == Priority.Must).OrderBy(i => i.SortOrder).ToList().AsReadOnly();

    public IReadOnlyList<RouteItem> GetOptionals()
        => _items.Where(i => i.Priority == Priority.Optional).OrderBy(i => i.SortOrder).ToList().AsReadOnly();

    private void RenumberItems()
    {
        for (int i = 0; i < _items.Count; i++)
            _items[i].SetSortOrder(i);
    }
}
