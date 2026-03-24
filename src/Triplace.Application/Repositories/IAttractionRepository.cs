using Triplace.Domain.Entities;
using Triplace.Domain.Ids;

namespace Triplace.Application.Repositories;

public interface IAttractionRepository
{
    Task<Attraction?> GetByIdAsync(AttractionId id, CancellationToken ct = default);
    Task<IReadOnlyList<Attraction>> GetAllAsync(CancellationToken ct = default);
    Task SaveAsync(Attraction attraction, CancellationToken ct = default);
    Task DeleteAsync(AttractionId id, CancellationToken ct = default);
}
