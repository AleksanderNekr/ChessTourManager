namespace ChessTourManager.Domain.ValueObjects;

public readonly ref struct DrawResult
{
    public enum ResultType
    {
        Success,
        NotEnoughPlayers,
        TournamentIsOver,
    }

    private DrawResult(ResultType result, ReadOnlySpan<char> message)
    {
        this.Result  = result;
        this.Message = message;
    }

    public ResultType Result { get; }

    public ReadOnlySpan<char> Message { get; }

    public static DrawResult Success(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.Success, message);
    }

    public static DrawResult NotEnoughPlayers(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.NotEnoughPlayers, message);
    }

    public static DrawResult TournamentIsOver(ReadOnlySpan<char> message)
    {
        return new DrawResult(ResultType.TournamentIsOver, message);
    }
}
