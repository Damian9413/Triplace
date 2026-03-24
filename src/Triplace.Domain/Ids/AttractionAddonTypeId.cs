namespace Triplace.Domain.Ids;

public readonly record struct AttractionAddonTypeId(Guid Value)
{
    public static AttractionAddonTypeId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
