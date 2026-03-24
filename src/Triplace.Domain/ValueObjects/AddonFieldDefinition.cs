using Triplace.Domain.Enums;

namespace Triplace.Domain.ValueObjects;

public class AddonFieldDefinition
{
    public string FieldName { get; }
    public FieldConstraint Constraint { get; }
    public FieldValueType ValueType { get; }

    public AddonFieldDefinition(string fieldName, FieldConstraint constraint, FieldValueType valueType)
    {
        FieldName = fieldName;
        Constraint = constraint;
        ValueType = valueType;
    }
}
