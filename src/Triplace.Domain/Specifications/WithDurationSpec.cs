using Triplace.Domain.Entities;
using Triplace.Domain.Enums;

namespace Triplace.Domain.Specifications;

public class WithDurationSpec(VisitDuration duration) : BaseSpecification<Attraction>
{
    public override bool IsSatisfiedBy(Attraction candidate) => candidate.Duration == duration;
}
