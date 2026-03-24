namespace Triplace.Domain.Ids;

public readonly record struct CatalogEntryId(Guid Value)
{
    public static CatalogEntryId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
