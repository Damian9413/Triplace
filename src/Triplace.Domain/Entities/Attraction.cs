namespace Triplace.Domain.Entities;

public class Attraction
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public List<string> Tags { get; init; } = [];
}
