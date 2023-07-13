using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class GamePair : IEquatable<GamePair>
{
    public GamePair(in Player white, in Player black)
    {
        this.White  = white;
        this.Black  = black;
        this.Result = GameResult.NotPlayed;
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
            && this.Black.Equals(other.Black);
    }

    public override string ToString()
    {
        return $"{this.White} - {this.Black}";
    }

    private void SetResult(in GameResult result)
    {
        this.Result = result;
        this.White.AddGameToHistory(this);
        this.Black.AddGameToHistory(this);
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

            // Impossible to have two games with the same players.
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

    internal enum GameResult
    {
        WhiteWin,
        BlackWin,
        Draw,
        WhiteWinByDefault,
        BlackWinByDefault,
        NotPlayed,
    }
}
