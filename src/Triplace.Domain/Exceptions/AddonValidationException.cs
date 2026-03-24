namespace Triplace.Domain.Exceptions;

public class AddonValidationException : DomainException
{
    public AddonValidationException(string message) : base(message) { }
}
