using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.Interfaces;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class Attraction : IAttractionNode
{
    public AttractionId Id { get; }
    public string Name { get; private set; }
    public AttractionStatus Status { get; private set; }
    public AttractionMetadata Metadata { get; private set; }

    private Attraction(AttractionId id, string name, AttractionMetadata metadata)
    {
        Id = id;
        Name = name;
        Status = AttractionStatus.Draft;
        Metadata = metadata;
    }

    internal static Attraction CreateDraft(string name, AttractionMetadata metadata)
        => new(AttractionId.New(), name, metadata);

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
