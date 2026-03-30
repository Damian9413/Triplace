using Triplace.Domain.Entities;
using Triplace.Domain.Enums;

namespace Triplace.Domain.Specifications;

public class HasAmenitySpec(AttractionAmenity amenity) : BaseSpecification<Attraction>
{
    public override bool IsSatisfiedBy(Attraction candidate) => candidate.Amenities.Contains(amenity);
}
