namespace Triplace.Domain.ValueObjects;

public class AllowedValuesConstraint : FieldConstraint
{
    public string[] AllowedValues { get; }

    public AllowedValuesConstraint(string[] allowedValues)
    {
        AllowedValues = allowedValues;
    }

    public override bool Validate(object value)
    {
        if (value is not string strValue) return false;
        return AllowedValues.Contains(strValue, StringComparer.Ordinal);
    }
}
