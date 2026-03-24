using Triplace.Domain.Entities;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Application.Repositories;

public interface ISeasonalCatalogRepository
{
    Task<SeasonalCatalog?> GetByIdAsync(SeasonalCatalogId id, CancellationToken ct = default);
    Task<IReadOnlyList<SeasonalCatalog>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SeasonalCatalog>> GetBySeasonAsync(Season season, CancellationToken ct = default);
    Task SaveAsync(SeasonalCatalog catalog, CancellationToken ct = default);
    Task DeleteAsync(SeasonalCatalogId id, CancellationToken ct = default);
}
