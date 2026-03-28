using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.Interfaces;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class Attraction : IAttractionNode
{
    private readonly HashSet<Season> _bestSeasons;

    public AttractionId Id { get; }
    public string Name { get; private set; }
    public AttractionStatus Status { get; private set; }
    public AttractionCategory Category { get; private set; }
    public IReadOnlySet<Season> BestSeasons => _bestSeasons;
    public VisitDuration Duration { get; private set; }
    public bool IsOutdoor { get; private set; }
    public bool IsFree { get; private set; }
    public AttractionMetadata Metadata { get; private set; }

    private Attraction(AttractionId id, string name, AttractionCategory category,
        HashSet<Season> bestSeasons, VisitDuration duration, bool isOutdoor, bool isFree,
        AttractionMetadata metadata)
    {
        Id = id;
        Name = name;
        Status = AttractionStatus.Draft;
        Category = category;
        _bestSeasons = bestSeasons;
        Duration = duration;
        IsOutdoor = isOutdoor;
        IsFree = isFree;
        Metadata = metadata;
    }

    internal static Attraction CreateDraft(string name, AttractionCategory category,
        HashSet<Season> bestSeasons, VisitDuration duration, bool isOutdoor, bool isFree,
        AttractionMetadata metadata)
        => new(AttractionId.New(), name, category, bestSeasons, duration, isOutdoor, isFree, metadata);

    public void Publish()
    {
        if (Status != AttractionStatus.Draft)
            throw new InvalidStatusTransitionException(
                $"Cannot publish '{Name}': status is {Status}, expected Draft.");
        Status = AttractionStatus.Published;
    }

    public void Archive()
    {
        if (Status != AttractionStatus.Published)
            throw new InvalidStatusTransitionException(
                $"Cannot archive '{Name}': status is {Status}, expected Published.");
        Status = AttractionStatus.Archived;
    }

    public IReadOnlyList<Attraction> Flatten() => [this];
}
