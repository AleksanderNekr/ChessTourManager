namespace ChessTourManager.Domain.ValueObjects;

public readonly ref struct DrawResult
{
    public enum ResultType
    {
        Success,
        Fail,
    }

    private DrawResult(ResultType result, ReadOnlySpan<char> message)
    {
        this.Result  = result;
        this.Message = message;
    }

    public ResultType Result { get; }

    public ReadOnlySpan<char> Message { get; }

    public static DrawResult Success(ReadOnlySpan<char> message = default)
    {
        return new DrawResult(ResultType.Success, message);
    }

    public static DrawResult Fail(ReadOnlySpan<char> message = default)
    {
        return new DrawResult(ResultType.Fail, message);
    }
}
