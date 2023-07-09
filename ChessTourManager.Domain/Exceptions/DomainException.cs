namespace ChessTourManager.Domain.Exceptions;

public sealed class DomainException : Exception
{
    internal DomainException(string? message) : base(message)
    {

    }
}
