using Triplace.Domain.Entities;

namespace Triplace.Domain.Specifications;

public class IsOutdoorSpec : BaseSpecification<Attraction>
{
    public override bool IsSatisfiedBy(Attraction candidate) => candidate.IsOutdoor;
}
