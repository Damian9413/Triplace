namespace Triplace.Domain.ValueObjects;

public class DateRangeConstraint : FieldConstraint
{
    public DateOnly From { get; }
    public DateOnly To { get; }

    public DateRangeConstraint(DateOnly from, DateOnly to)
    {
        From = from;
        To = to;
    }

    public override bool Validate(object value)
    {
        if (value is not DateOnly date) return false;
        return date >= From && date <= To;
    }
}
