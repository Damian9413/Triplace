using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Domain.Entities;

public class RouteItem
{
    public RouteItemId Id { get; }
    public CatalogEntryId CatalogEntryId { get; }
    public Priority Priority { get; }
    public int SortOrder { get; private set; }

    internal RouteItem(RouteItemId id, CatalogEntryId catalogEntryId, Priority priority, int sortOrder)
    {
        Id = id;
        CatalogEntryId = catalogEntryId;
        Priority = priority;
        SortOrder = sortOrder;
    }

    internal void SetSortOrder(int sortOrder) => SortOrder = sortOrder;
}
