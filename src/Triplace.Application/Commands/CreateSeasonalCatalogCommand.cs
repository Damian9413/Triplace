using Triplace.Domain.Enums;

namespace Triplace.Application.Commands;

public record CreateSeasonalCatalogCommand(
    string Name,
    Season Season,
    DateOnly ValidFrom,
    DateOnly ValidTo,
    string Region,
    string Description,
    int? MaxCapacity = null);
