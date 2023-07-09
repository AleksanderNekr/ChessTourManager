namespace ChessTourManager.Domain.Exceptions;

internal sealed class DomainException : Exception
{
    internal DomainException(string? message) : base(message)
    {

    }
}
