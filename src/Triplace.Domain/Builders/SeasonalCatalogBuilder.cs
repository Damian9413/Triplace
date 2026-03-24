using Triplace.Domain.Entities;

namespace Triplace.Domain.Builders;

public class SeasonalCatalogBuilder
{
    private readonly string _name;
    private readonly CatalogMetadataBuilder _metadataBuilder = new();

    private SeasonalCatalogBuilder(string name) => _name = name;

    public static SeasonalCatalogBuilder Create(string name) => new(name);

    public SeasonalCatalogBuilder WithMetadata(Action<CatalogMetadataBuilder> configure)
    {
        configure(_metadataBuilder);
        return this;
    }

    public SeasonalCatalog Build()
        => SeasonalCatalog.Create(_name, _metadataBuilder.Build());
}
