using Triplace.Domain.Entities;
using Triplace.Domain.Ids;

namespace Triplace.Application.Repositories;

public interface IAttractionGroupRepository
{
    Task<AttractionGroup?> GetByIdAsync(AttractionGroupId id, CancellationToken ct = default);
    Task<IReadOnlyList<AttractionGroup>> GetAllAsync(CancellationToken ct = default);
    Task SaveAsync(AttractionGroup group, CancellationToken ct = default);
    Task DeleteAsync(AttractionGroupId id, CancellationToken ct = default);
}
