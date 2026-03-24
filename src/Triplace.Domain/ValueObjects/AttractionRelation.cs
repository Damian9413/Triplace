using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Domain.ValueObjects;

public class AttractionRelation : IEquatable<AttractionRelation>
{
    public AttractionId LowId { get; }
    public AttractionId HighId { get; }
    public AttractionRelationType Type { get; }

    internal AttractionRelation(AttractionId a, AttractionId b, AttractionRelationType type)
    {
        if (a.Value.CompareTo(b.Value) <= 0)
        {
            LowId = a;
            HighId = b;
        }
        else
        {
            LowId = b;
            HighId = a;
        }
        Type = type;
    }

    public bool Involves(AttractionId id) => LowId == id || HighId == id;

    public AttractionId Other(AttractionId id) => LowId == id ? HighId : LowId;

    public bool Equals(AttractionRelation? other)
    {
        if (other is null) return false;
        return LowId == other.LowId && HighId == other.HighId && Type == other.Type;
    }

    public override bool Equals(object? obj) => obj is AttractionRelation r && Equals(r);

    public override int GetHashCode() => HashCode.Combine(LowId, HighId, Type);
}
