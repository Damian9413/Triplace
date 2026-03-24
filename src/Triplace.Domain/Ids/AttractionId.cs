namespace Triplace.Domain.Ids;

public readonly record struct AttractionId(Guid Value)
{
    public static AttractionId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
