using Triplace.Domain.Enums;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Specifications;

public class RelationOfTypeSpec : BaseSpecification<AttractionRelation>
{
    private readonly AttractionRelationType _type;

    public RelationOfTypeSpec(AttractionRelationType type) => _type = type;

    public override bool IsSatisfiedBy(AttractionRelation candidate) => candidate.Type == _type;
}
