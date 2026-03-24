using System.Collections.Concurrent;
using Triplace.Application.Repositories;
using Triplace.Domain.Ids;

namespace Triplace.Infrastructure.Repositories;

public class InMemoryRouteRepository : IRouteRepository
{
    private readonly ConcurrentDictionary<Guid, Domain.Entities.Route> _store = new();

    public Task<Domain.Entities.Route?> GetByIdAsync(RouteId id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id.Value, out var r) ? r : null);

    public Task<IReadOnlyList<Domain.Entities.Route>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Domain.Entities.Route>>(_store.Values.ToList());

    public Task SaveAsync(Domain.Entities.Route route, CancellationToken ct = default)
    {
        _store[route.Id.Value] = route;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(RouteId id, CancellationToken ct = default)
    {
        _store.TryRemove(id.Value, out _);
        return Task.CompletedTask;
    }
}
