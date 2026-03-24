using Triplace.Domain.Entities;

namespace Triplace.Application.Repositories;

public interface IAttractionRelationRegistryRepository
{
    Task<AttractionRelationRegistry> GetRegistryAsync(CancellationToken ct = default);
    Task SaveAsync(AttractionRelationRegistry registry, CancellationToken ct = default);
}
