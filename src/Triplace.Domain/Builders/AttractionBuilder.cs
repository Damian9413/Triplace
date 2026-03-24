using Triplace.Domain.Entities;

namespace Triplace.Domain.Builders;

public class AttractionBuilder
{
    private readonly string _name;
    private readonly AttractionMetadataBuilder _metadataBuilder = new();

    private AttractionBuilder(string name) => _name = name;

    public static AttractionBuilder CreateDraft(string name) => new(name);

    public AttractionBuilder WithMetadata(Action<AttractionMetadataBuilder> configure)
    {
        configure(_metadataBuilder);
        return this;
    }

    public Attraction Build() => Attraction.CreateDraft(_name, _metadataBuilder.Build());
}
