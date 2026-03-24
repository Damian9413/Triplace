using System.Collections.Concurrent;
using Triplace.Application.Repositories;
using Triplace.Domain.Entities;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Infrastructure.Repositories;

public class InMemorySeasonalCatalogRepository : ISeasonalCatalogRepository
{
    private readonly ConcurrentDictionary<Guid, SeasonalCatalog> _store = new();

    public Task<SeasonalCatalog?> GetByIdAsync(SeasonalCatalogId id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id.Value, out var c) ? c : null);

    public Task<IReadOnlyList<SeasonalCatalog>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<SeasonalCatalog>>(_store.Values.ToList());

    public Task<IReadOnlyList<SeasonalCatalog>> GetBySeasonAsync(Season season, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<SeasonalCatalog>>(
            _store.Values.Where(c => c.Metadata.Season == season).ToList());

    public Task SaveAsync(SeasonalCatalog catalog, CancellationToken ct = default)
    {
        _store[catalog.Id.Value] = catalog;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(SeasonalCatalogId id, CancellationToken ct = default)
    {
        _store.TryRemove(id.Value, out _);
        return Task.CompletedTask;
    }
}
