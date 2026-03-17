using Triplace.Domain.Entities;
using Triplace.Domain.Repositories;

namespace Triplace.Infrastructure.Repositories;

public class InMemoryAttractionRepository : IAttractionRepository
{
    private static readonly List<Attraction> _attractions =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Wawel",
            Description = "Historyczny zamek królewski na wzgórzu wawelskim.",
            City = "Kraków",
            Latitude = 50.0540,
            Longitude = 19.9355,
            Tags = ["zamek", "historia", "widok"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Rynek Główny",
            Description = "Największy średniowieczny rynek w Europie, centrum Starego Miasta.",
            City = "Kraków",
            Latitude = 50.0617,
            Longitude = 19.9373,
            Tags = ["rynek", "historia", "architektura"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Kazimierz",
            Description = "Historyczna dzielnica żydowska z klimatycznymi kawiarniami i galeriami.",
            City = "Kraków",
            Latitude = 50.0514,
            Longitude = 19.9453,
            Tags = ["kultura", "historia", "gastronomia"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Muzeum Narodowe w Krakowie",
            Description = "Największe muzeum sztuki i historii w Krakowie.",
            City = "Kraków",
            Latitude = 50.0611,
            Longitude = 19.9168,
            Tags = ["muzeum", "sztuka", "kultura"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Pałac Kultury i Nauki",
            Description = "Ikoniczny socrealistyczny wieżowiec z tarasem widokowym.",
            City = "Warszawa",
            Latitude = 52.2317,
            Longitude = 21.0058,
            Tags = ["architektura", "widok", "historia"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Stare Miasto Warszawa",
            Description = "Odbudowane po wojnie historyczne centrum z Rynkiem Starego Miasta.",
            City = "Warszawa",
            Latitude = 52.2497,
            Longitude = 21.0122,
            Tags = ["historia", "architektura", "UNESCO"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Łazienki Królewskie",
            Description = "Piękny park z Pałacem na Wodzie i pomnikiem Chopina.",
            City = "Warszawa",
            Latitude = 52.2150,
            Longitude = 21.0355,
            Tags = ["park", "pałac", "przyroda"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Muzeum Powstania Warszawskiego",
            Description = "Nowoczesne muzeum poświęcone Powstaniu Warszawskiemu z 1944 roku.",
            City = "Warszawa",
            Latitude = 52.2323,
            Longitude = 20.9812,
            Tags = ["muzeum", "historia", "II Wojna Światowa"]
        }
    ];

    public Task<IEnumerable<Attraction>> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        var result = _attractions
            .Where(a => string.Equals(a.City, city, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(result);
    }
}
