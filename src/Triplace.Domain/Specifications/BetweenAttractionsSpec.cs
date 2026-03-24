using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Specifications;

public class BetweenAttractionsSpec : BaseSpecification<AttractionRelation>
{
    private readonly AttractionId _a;
    private readonly AttractionId _b;

    public BetweenAttractionsSpec(AttractionId a, AttractionId b) { _a = a; _b = b; }

    public override bool IsSatisfiedBy(AttractionRelation candidate)
        => candidate.Involves(_a) && candidate.Involves(_b);
}
