using System.Collections.Concurrent;
using Triplace.Application.Repositories;
using Triplace.Domain.Entities;
using Triplace.Domain.Ids;

namespace Triplace.Infrastructure.Repositories;

public class InMemoryAttractionRepository : IAttractionRepository
{
    private readonly ConcurrentDictionary<Guid, Attraction> _store = new();

    public Task<Attraction?> GetByIdAsync(AttractionId id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id.Value, out var a) ? a : null);

    public Task<IReadOnlyList<Attraction>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Attraction>>(_store.Values.ToList());

    public Task SaveAsync(Attraction attraction, CancellationToken ct = default)
    {
        _store[attraction.Id.Value] = attraction;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AttractionId id, CancellationToken ct = default)
    {
        _store.TryRemove(id.Value, out _);
        return Task.CompletedTask;
    }
}
