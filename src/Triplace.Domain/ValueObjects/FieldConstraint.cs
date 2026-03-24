namespace Triplace.Domain.ValueObjects;

public abstract class FieldConstraint
{
    public abstract bool Validate(object value);
}
