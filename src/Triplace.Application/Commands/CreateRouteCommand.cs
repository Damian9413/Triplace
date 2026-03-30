using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Application.Commands;

public record CreateRouteCommand(
    string Name,
    string Description,
    Season Season,
    AttractionId? ScopeAttractionId,
    IReadOnlyList<RouteItemCommand> Items);

public record RouteItemCommand(AttractionId AttractionId, Priority Priority);
