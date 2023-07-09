namespace ChessTourManager.Domain.Exceptions;

internal sealed class DomainOutOfRangeException : ArgumentOutOfRangeException
{
    internal DomainOutOfRangeException(string? paramName, object? actualValue, string? message = null)
        : base(paramName, actualValue, message)
    {

    }
}
