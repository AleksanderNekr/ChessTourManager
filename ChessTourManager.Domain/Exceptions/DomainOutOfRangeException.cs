namespace ChessTourManager.Domain.Exceptions;

public class DomainOutOfRangeException : ArgumentOutOfRangeException
{
    public DomainOutOfRangeException(string? paramName, object? actualValue, string? message = null)
        : base(paramName, actualValue, message)
    {

    }
}
