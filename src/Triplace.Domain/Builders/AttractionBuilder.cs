using Triplace.Domain.Entities;
using Triplace.Domain.Enums;

namespace Triplace.Domain.Builders;

public class AttractionBuilder
{
    private readonly string _name;
    private AttractionCategory _category;
    private readonly HashSet<Season> _bestSeasons = [];
    private VisitDuration _duration;
    private bool _isOutdoor;
    private bool _isFree;
    private readonly HashSet<AttractionAmenity> _amenities = [];
    private readonly AttractionMetadataBuilder _metadataBuilder = new();

    private AttractionBuilder(string name) => _name = name;

    public static AttractionBuilder CreateDraft(string name) => new(name);

    public AttractionBuilder InCategory(AttractionCategory category) { _category = category; return this; }
    public AttractionBuilder BestIn(params Season[] seasons) { foreach (var s in seasons) _bestSeasons.Add(s); return this; }
    public AttractionBuilder WithDuration(VisitDuration duration) { _duration = duration; return this; }
    public AttractionBuilder Outdoor() { _isOutdoor = true; return this; }
    public AttractionBuilder Free() { _isFree = true; return this; }
    public AttractionBuilder WithAmenities(params AttractionAmenity[] amenities) { foreach (var a in amenities) _amenities.Add(a); return this; }

    public AttractionBuilder WithMetadata(Action<AttractionMetadataBuilder> configure)
    {
        configure(_metadataBuilder);
        return this;
    }

    public Attraction Build() =>
        Attraction.CreateDraft(_name, _category, _bestSeasons, _duration, _isOutdoor, _isFree, _amenities, _metadataBuilder.Build());
}
