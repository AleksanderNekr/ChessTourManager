namespace ChessTourManager.Domain.Entities;

public sealed class GamePair<TPlayer> : IEquatable<GamePair<TPlayer>> where TPlayer : Participant<TPlayer>
{
    internal GamePair(in TPlayer white, in TPlayer black, in GameResult result = GameResult.NotYetPlayed)
    {
        this.White = white;
        this.Black = black;
        this.SetResult(result);
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

        return this.White.Equals(other.White)
            && this.Black.Equals(other.Black)
            && this.Result == other.Result;
    }

    public override string ToString()
    {
        return $"{this.White} - {this.Black}: {this.Result}";
    }

    private void SetResult(in GameResult result)
    {
        this.Result = result;
        this.White.AddGameToHistory(this, PlayerColor.White);
        this.Black.AddGameToHistory(this, PlayerColor.Black);
    }

    public override bool Equals(object? obj)
    {
        return obj is GamePair<TPlayer> other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.White, this.Black);
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
