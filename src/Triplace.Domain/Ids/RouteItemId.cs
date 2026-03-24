namespace Triplace.Domain.Ids;

public readonly record struct RouteItemId(Guid Value)
{
    public static RouteItemId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
