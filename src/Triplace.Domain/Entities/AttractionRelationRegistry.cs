using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class AttractionRelationRegistry
{
    private readonly HashSet<AttractionRelation> _relations = new();

    public IReadOnlyCollection<AttractionRelation> All => _relations;

    public void AddExclusion(AttractionId a, AttractionId b)
        => _relations.Add(new AttractionRelation(a, b, AttractionRelationType.Exclusion));

    public void AddRecommendation(AttractionId a, AttractionId b)
        => _relations.Add(new AttractionRelation(a, b, AttractionRelationType.Recommendation));

    public void Remove(AttractionId a, AttractionId b, AttractionRelationType type)
    {
        var toRemove = new AttractionRelation(a, b, type);
        _relations.RemoveWhere(r => r.Equals(toRemove));
    }

    public IReadOnlyList<AttractionRelation> GetRelations(AttractionId id)
        => _relations.Where(r => r.Involves(id)).ToList().AsReadOnly();

    public bool AreExclusive(AttractionId a, AttractionId b)
        => _relations.Any(r =>
            r.Involves(a) && r.Involves(b) && r.Type == AttractionRelationType.Exclusion);
}
