using Triplace.Domain.Enums;
using Triplace.Domain.ValueObjects;

namespace Triplace.Application.Commands;

public record CreateAttractionCommand(
    string Name,
    AttractionCategory Category,
    IReadOnlySet<Season> BestSeasons,
    VisitDuration Duration,
    bool IsOutdoor,
    bool IsFree,
    IReadOnlyList<MetadataEntry> MetadataEntries);
