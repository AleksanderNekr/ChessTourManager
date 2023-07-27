namespace ChessTourManager.Domain.ValueObjects;

public readonly ref struct DrawResult
{
    public enum ResultType
    {
        Success,
        NotEnoughPlayers,
        TournamentIsOver,
        Fail,
    }

    private DrawResult(ResultType result, ReadOnlySpan<char> message)
    {
        Result  = result;
        Message = message;
    }

    public ResultType Result { get; }

    public ReadOnlySpan<char> Message { get; }

    internal static DrawResult Success(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.Success, message);
    }

    internal static DrawResult NotEnoughPlayers(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.NotEnoughPlayers, message);
    }

    internal static DrawResult TournamentIsOver(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.TournamentIsOver, message);
    }

    internal static DrawResult Fail(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.Fail, message);
    }
}
