using Triplace.Application.Repositories;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Application.Services;

public class RelationService(IAttractionRelationRegistryRepository registryRepository)
{
    public async Task AddExclusionAsync(AttractionId a, AttractionId b)
    {
        var registry = await registryRepository.GetRegistryAsync();
        registry.AddExclusion(a, b);
        await registryRepository.SaveAsync(registry);
    }

    public async Task AddRecommendationAsync(AttractionId a, AttractionId b)
    {
        var registry = await registryRepository.GetRegistryAsync();
        registry.AddRecommendation(a, b);
        await registryRepository.SaveAsync(registry);
    }

    public async Task RemoveAsync(AttractionId a, AttractionId b, AttractionRelationType type)
    {
        var registry = await registryRepository.GetRegistryAsync();
        registry.Remove(a, b, type);
        await registryRepository.SaveAsync(registry);
    }

    public async Task<IReadOnlyList<AttractionRelation>> GetRelationsForAsync(AttractionId id)
    {
        var registry = await registryRepository.GetRegistryAsync();
        return registry.GetRelations(id);
    }

    public async Task<bool> AreExclusiveAsync(AttractionId a, AttractionId b)
    {
        var registry = await registryRepository.GetRegistryAsync();
        return registry.AreExclusive(a, b);
    }

}
