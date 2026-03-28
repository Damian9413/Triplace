using Triplace.Domain.Entities;
using Triplace.Domain.Enums;

namespace Triplace.Domain.Specifications;

public class InCategorySpec(AttractionCategory category) : BaseSpecification<Attraction>
{
    public override bool IsSatisfiedBy(Attraction candidate) => candidate.Category == category;
}
