using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Application.Commands;

public record CreateRouteCommand(
    string Name,
    string Description,
    Season Season,
    AttractionGroupId? ScopeGroupId,
    IReadOnlyList<RouteItemCommand> Items);

public record RouteItemCommand(CatalogEntryId CatalogEntryId, Priority Priority);
