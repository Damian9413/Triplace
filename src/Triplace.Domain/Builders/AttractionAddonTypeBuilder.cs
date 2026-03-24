using Triplace.Domain.Entities;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Builders;

public class AttractionAddonTypeBuilder
{
    private readonly string _name;
    private readonly List<AddonFieldDefinition> _fields = new();

    private AttractionAddonTypeBuilder(string name) => _name = name;

    public static AttractionAddonTypeBuilder Create(string name) => new(name);

    public AttractionAddonTypeBuilder WithField(string fieldName, FieldValueType valueType, FieldConstraint constraint)
    {
        _fields.Add(new AddonFieldDefinition(fieldName, constraint, valueType));
        return this;
    }

    public AttractionAddonType Build()
        => new(AttractionAddonTypeId.New(), _name, _fields);
}
