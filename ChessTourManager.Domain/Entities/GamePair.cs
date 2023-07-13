namespace ChessTourManager.Domain.Entities;

public sealed class GamePair : IEquatable<GamePair>
{
    public enum GameResult
    {
        WhiteWin,
        BlackWin,
        Draw,
        WhiteWinByDefault,
        BlackWinByDefault,
        BothLeave,
        NotYetPlayed,
    }

    public GamePair(in Player white, in Player black, in GameResult result = GameResult.NotYetPlayed)
    {
        this.White = white;
        this.Black = black;
        this.SetResult(result);
    }

    internal Player White { get; }

    internal Player Black { get; }

    internal GameResult Result { get; private set; }

    public bool Equals(GamePair? other)
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
        return $"{this.White} - {this.Black}";
    }

    private void SetResult(in GameResult result)
    {
        this.Result = result;
        this.White.AddGameToHistory(this, PlayerColor.White);
        this.Black.AddGameToHistory(this, PlayerColor.Black);
    }

    public override bool Equals(object? obj)
    {
        return obj is GamePair other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.White, this.Black);
    }

    /// <inheritdoc />
    /// <summary>
    ///     Compares two <see cref="T:ChessTourManager.Domain.Entities.GamePair" /> objects for equality by their players.
    /// </summary>
    internal sealed class ByPlayersEqualityComparer : IEqualityComparer<GamePair>
    {
        public bool Equals(GamePair? x, GamePair? y)
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

        public int GetHashCode(GamePair obj)
        {
            return HashCode.Combine(obj.White, obj.Black);
        }
    }

    internal enum PlayerColor
    {
        White,
        Black,
    }
}
