using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Specifications;

public class InvolvesAttractionSpec : BaseSpecification<AttractionRelation>
{
    private readonly AttractionId _id;

    public InvolvesAttractionSpec(AttractionId id) => _id = id;

    public override bool IsSatisfiedBy(AttractionRelation candidate) => candidate.Involves(_id);
}
