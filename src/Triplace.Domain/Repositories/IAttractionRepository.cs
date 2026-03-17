using Triplace.Domain.Entities;

namespace Triplace.Domain.Repositories;

public interface IAttractionRepository
{
    Task<IEnumerable<Attraction>> GetByCityAsync(string city, CancellationToken cancellationToken = default);
}
