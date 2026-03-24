using Triplace.Domain.Entities;
using Triplace.Domain.Ids;

namespace Triplace.Application.Repositories;

public interface IAttractionAddonTypeRepository
{
    Task<AttractionAddonType?> GetByIdAsync(AttractionAddonTypeId id, CancellationToken ct = default);
    Task<IReadOnlyList<AttractionAddonType>> GetAllAsync(CancellationToken ct = default);
    Task SaveAsync(AttractionAddonType addonType, CancellationToken ct = default);
    Task DeleteAsync(AttractionAddonTypeId id, CancellationToken ct = default);
}
