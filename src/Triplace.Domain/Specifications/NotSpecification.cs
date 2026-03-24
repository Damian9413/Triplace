namespace Triplace.Domain.Specifications;

public class NotSpecification<T> : BaseSpecification<T>
{
    private readonly ISpecification<T> _inner;

    public NotSpecification(ISpecification<T> inner) => _inner = inner;

    public override bool IsSatisfiedBy(T candidate) => !_inner.IsSatisfiedBy(candidate);
}
