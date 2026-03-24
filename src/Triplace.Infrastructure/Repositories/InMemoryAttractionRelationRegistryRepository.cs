using Triplace.Application.Repositories;
using Triplace.Domain.Entities;

namespace Triplace.Infrastructure.Repositories;

public class InMemoryAttractionRelationRegistryRepository : IAttractionRelationRegistryRepository
{
    private readonly AttractionRelationRegistry _registry = new();

    public Task<AttractionRelationRegistry> GetRegistryAsync(CancellationToken ct = default)
        => Task.FromResult(_registry);

    public Task SaveAsync(AttractionRelationRegistry registry, CancellationToken ct = default)
        => Task.CompletedTask; // singleton — already updated in-place
}
