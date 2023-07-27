namespace ChessTourManager.Domain.Entities;

public sealed class GamePair<TPlayer> : IEquatable<GamePair<TPlayer>> where TPlayer : Participant<TPlayer>
{
    internal GamePair(in TPlayer white, in TPlayer black, in GameResult result = GameResult.NotYetPlayed)
    {
        White = white;
        Black = black;
        SetResult(result);
    }

    internal TPlayer White { get; }

    internal TPlayer Black { get; }

    internal GameResult Result { get; private set; }

    public bool Equals(GamePair<TPlayer>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return White.Equals(other.White)
            && Black.Equals(other.Black)
            && Result == other.Result;
    }

    public override string ToString()
    {
        return $"{White} - {Black}: {Result}";
    }

    private void SetResult(in GameResult result)
    {
        Result = result;
        White.AddGameToHistory(this, PlayerColor.White);
        Black.AddGameToHistory(this, PlayerColor.Black);
    }

    public override bool Equals(object? obj)
    {
        return obj is GamePair<TPlayer> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(White, Black);
    }

    /// <summary>
    ///     Compares two <see cref="T:ChessTourManager.Domain.Entities.GamePair" /> objects for equality by their players.
    /// </summary>
    /// <inheritdoc />
    internal sealed class ByPlayersEqualityComparer : IEqualityComparer<GamePair<TPlayer>>
    {
        public bool Equals(GamePair<TPlayer>? x, GamePair<TPlayer>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.White.Equals(y.White) && x.Black.Equals(y.Black)
                || x.White.Equals(y.Black) && x.Black.Equals(y.White);
        }

        public int GetHashCode(GamePair<TPlayer> obj)
        {
            return HashCode.Combine(obj.White, obj.Black);
        }
    }
}

internal enum GameResult
{
    WhiteWin,
    BlackWin,
    Draw,
    WhiteWinByDefault,
    BlackWinByDefault,
    BothLeave,
    NotYetPlayed,
}

internal enum PlayerColor
{
    White,
    Black,
}
