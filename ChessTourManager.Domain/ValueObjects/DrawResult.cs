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
		Result = result;
		Message = message;
	}

	public ResultType Result { get; }

	public ReadOnlySpan<char> Message { get; }

	internal static DrawResult Success(ReadOnlySpan<char> message)
		=> new(ResultType.Success, message);

	internal static DrawResult NotEnoughPlayers(ReadOnlySpan<char> message)
		=> new(ResultType.NotEnoughPlayers, message);

	internal static DrawResult TournamentIsOver(ReadOnlySpan<char> message)
		=> new(ResultType.TournamentIsOver, message);

	internal static DrawResult Fail(ReadOnlySpan<char> message)
		=> new(ResultType.Fail, message);
}