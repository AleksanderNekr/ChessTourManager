namespace Domain.ValueObjects;

public readonly ref struct DrawResult
{
    public enum ResultType
    {
        Success,
        Fail,
    }

    public required ResultType Type { get; init; }

    public ReadOnlySpan<char> Message { get; init; }
}
