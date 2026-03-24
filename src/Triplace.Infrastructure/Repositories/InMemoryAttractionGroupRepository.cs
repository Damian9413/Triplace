using System.Collections.Concurrent;
using Triplace.Application.Repositories;
using Triplace.Domain.Entities;
using Triplace.Domain.Ids;

namespace Triplace.Infrastructure.Repositories;

public class InMemoryAttractionGroupRepository : IAttractionGroupRepository
{
    private readonly ConcurrentDictionary<Guid, AttractionGroup> _store = new();

    public Task<AttractionGroup?> GetByIdAsync(AttractionGroupId id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id.Value, out var g) ? g : null);

    public Task<IReadOnlyList<AttractionGroup>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<AttractionGroup>>(_store.Values.ToList());

    public Task SaveAsync(AttractionGroup group, CancellationToken ct = default)
    {
        _store[group.Id.Value] = group;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AttractionGroupId id, CancellationToken ct = default)
    {
        _store.TryRemove(id.Value, out _);
        return Task.CompletedTask;
    }
}
