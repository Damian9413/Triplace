using Triplace.Domain.Entities;

namespace Triplace.Domain.Specifications;

public class IsFreeSpec : BaseSpecification<Attraction>
{
    public override bool IsSatisfiedBy(Attraction candidate) => candidate.IsFree;
}
