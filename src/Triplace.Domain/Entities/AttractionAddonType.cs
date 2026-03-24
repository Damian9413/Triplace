using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Domain.Entities;

public class AttractionAddonType
{
    private readonly List<AddonFieldDefinition> _fields;

    public AttractionAddonTypeId Id { get; }
    public string Name { get; }
    public IReadOnlyList<AddonFieldDefinition> Fields => _fields.AsReadOnly();

    internal AttractionAddonType(AttractionAddonTypeId id, string name, List<AddonFieldDefinition> fields)
    {
        Id = id;
        Name = name;
        _fields = fields;
    }

    public void Validate(Dictionary<string, object> values)
    {
        foreach (var field in _fields)
        {
            if (!values.TryGetValue(field.FieldName, out var value))
                throw new AddonValidationException(
                    $"Missing required field '{field.FieldName}' for addon type '{Name}'.");

            if (!field.Constraint.Validate(value))
                throw new AddonValidationException(
                    $"Invalid value for field '{field.FieldName}' in addon type '{Name}'.");
        }
    }
}
