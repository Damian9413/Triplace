namespace Triplace.Domain.Ids;

public readonly record struct AttractionGroupId(Guid Value)
{
    public static AttractionGroupId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
