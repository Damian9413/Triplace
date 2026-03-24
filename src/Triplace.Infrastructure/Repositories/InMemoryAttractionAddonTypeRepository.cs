using System.Collections.Concurrent;
using Triplace.Application.Repositories;
using Triplace.Domain.Entities;
using Triplace.Domain.Ids;

namespace Triplace.Infrastructure.Repositories;

public class InMemoryAttractionAddonTypeRepository : IAttractionAddonTypeRepository
{
    private readonly ConcurrentDictionary<Guid, AttractionAddonType> _store = new();

    public Task<AttractionAddonType?> GetByIdAsync(AttractionAddonTypeId id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id.Value, out var t) ? t : null);

    public Task<IReadOnlyList<AttractionAddonType>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<AttractionAddonType>>(_store.Values.ToList());

    public Task SaveAsync(AttractionAddonType addonType, CancellationToken ct = default)
    {
        _store[addonType.Id.Value] = addonType;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AttractionAddonTypeId id, CancellationToken ct = default)
    {
        _store.TryRemove(id.Value, out _);
        return Task.CompletedTask;
    }
}
