namespace Triplace.Domain.Ids;

public readonly record struct RouteId(Guid Value)
{
    public static RouteId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
