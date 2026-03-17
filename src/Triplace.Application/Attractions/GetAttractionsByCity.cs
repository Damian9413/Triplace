using Triplace.Domain.Entities;
using Triplace.Domain.Repositories;

namespace Triplace.Application.Attractions;

public class GetAttractionsByCity(IAttractionRepository repository)
{
    public Task<IEnumerable<Attraction>> ExecuteAsync(string city, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City name cannot be empty.", nameof(city));

        return repository.GetByCityAsync(city, cancellationToken);
    }
}
