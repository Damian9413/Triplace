using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Domain.Entities;

public class RouteItem
{
    public RouteItemId Id { get; }
    public AttractionId AttractionId { get; }
    public Priority Priority { get; }
    public int SortOrder { get; private set; }

    internal RouteItem(RouteItemId id, AttractionId attractionId, Priority priority, int sortOrder)
    {
        Id = id;
        AttractionId = attractionId;
        Priority = priority;
        SortOrder = sortOrder;
    }

    internal void SetSortOrder(int sortOrder) => SortOrder = sortOrder;
}
