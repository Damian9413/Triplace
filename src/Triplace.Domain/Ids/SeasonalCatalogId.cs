namespace Triplace.Domain.Ids;

public readonly record struct SeasonalCatalogId(Guid Value)
{
    public static SeasonalCatalogId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
