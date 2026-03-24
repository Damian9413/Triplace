namespace Triplace.Domain.Ids;

public readonly record struct AttractionEntryId(Guid Value)
{
    public static AttractionEntryId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
