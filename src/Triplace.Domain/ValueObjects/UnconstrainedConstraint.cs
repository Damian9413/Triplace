namespace Triplace.Domain.ValueObjects;

public class UnconstrainedConstraint : FieldConstraint
{
    public override bool Validate(object value) => true;
}
