using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class Attraction
{
    private readonly HashSet<Season> _bestSeasons;
    private readonly HashSet<AttractionAmenity> _amenities;
    private readonly List<Attraction> _children = new();
    private AttractionId? _parentId;

    public AttractionId Id { get; }
    public string Name { get; private set; }
    public AttractionStatus Status { get; private set; }
    public AttractionCategory Category { get; private set; }
    public IReadOnlySet<Season> BestSeasons => _bestSeasons;
    public VisitDuration Duration { get; private set; }
    public bool IsOutdoor { get; private set; }
    public bool IsFree { get; private set; }
    public IReadOnlySet<AttractionAmenity> Amenities => _amenities;
    public AttractionMetadata Metadata { get; private set; }
    public AttractionId? ParentId => _parentId;
    public IReadOnlyList<Attraction> Children => _children.AsReadOnly();
    public bool IsLeaf => _children.Count == 0;

    private Attraction(AttractionId id, string name, AttractionCategory category,
        HashSet<Season> bestSeasons, VisitDuration duration, bool isOutdoor, bool isFree,
        HashSet<AttractionAmenity> amenities, AttractionMetadata metadata)
    {
        Id = id;
        Name = name;
        Status = AttractionStatus.Draft;
        Category = category;
        _bestSeasons = bestSeasons;
        Duration = duration;
        IsOutdoor = isOutdoor;
        IsFree = isFree;
        _amenities = amenities;
        Metadata = metadata;
    }

    internal static Attraction CreateDraft(string name, AttractionCategory category,
        HashSet<Season> bestSeasons, VisitDuration duration, bool isOutdoor, bool isFree,
        HashSet<AttractionAmenity> amenities, AttractionMetadata metadata)
        => new(AttractionId.New(), name, category, bestSeasons, duration, isOutdoor, isFree, amenities, metadata);

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

    public void AddChild(Attraction child)
    {
        if (child._parentId.HasValue)
            throw new DomainException($"Attraction '{child.Name}' already has a parent.");
        child._parentId = Id;
        _children.Add(child);
    }

    public IReadOnlyList<Attraction> Flatten()
    {
        var result = new List<Attraction> { this };
        foreach (var child in _children)
            result.AddRange(child.Flatten());
        return result.AsReadOnly();
    }
}
