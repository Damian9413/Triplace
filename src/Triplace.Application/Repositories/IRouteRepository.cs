using Triplace.Domain.Ids;

namespace Triplace.Application.Repositories;

public interface IRouteRepository
{
    Task<Domain.Entities.Route?> GetByIdAsync(RouteId id, CancellationToken ct = default);
    Task<IReadOnlyList<Domain.Entities.Route>> GetAllAsync(CancellationToken ct = default);
    Task SaveAsync(Domain.Entities.Route route, CancellationToken ct = default);
    Task DeleteAsync(RouteId id, CancellationToken ct = default);
}
