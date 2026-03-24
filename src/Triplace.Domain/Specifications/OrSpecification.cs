namespace Triplace.Domain.Specifications;

public class OrSpecification<T> : BaseSpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override bool IsSatisfiedBy(T candidate)
        => _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
}
