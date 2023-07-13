using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Player : IEquatable<Player>
{
    private readonly HashSet<GamePair> _gamesHistory;
    private          decimal           _points;

    public Player(Id<Guid> id, Name name)
    {
        this.Id            = id;
        this.Name          = name;
        this._gamesHistory = new HashSet<GamePair>(comparer: new GamePair.ByPlayersEqualityComparer());
    }

    private Id<Guid> Id { get; }

    private Name Name { get; }

    private decimal Points
    {
        get => this._points;
        set
        {
            if (value < 0)
            {
                throw new DomainException("Points cannot be negative.");
            }

            this._points = value;
        }
    }

    private uint Wins { get; set; }

    private uint Draws { get; set; }

    private uint Loses { get; set; }

    public bool Equals(Player? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Id     == other.Id
            && this.Name   == other.Name
            && this.Points == other.Points
            && this.Wins   == other.Wins
            && this.Draws  == other.Draws
            && this.Loses  == other.Loses;
    }

    public IEnumerable<Player> GetAllOpponents()
    {
        foreach (GamePair pair in this._gamesHistory)
        {
            if (pair.White.Equals(this))
            {
                yield return pair.Black;
            }
            else
            {
                yield return pair.White;
            }
        }
    }

    public IEnumerable<Player> GetBlackOpponents()
    {
        return this._gamesHistory
                   .Where(pair => pair.White.Equals(this))
                   .Select(static pair => pair.Black);
    }

    public IEnumerable<Player> GetWhiteOpponents()
    {
        return this._gamesHistory
                   .Where(pair => pair.Black.Equals(this))
                   .Select(static pair => pair.White);
    }


    public override string ToString()
    {
        return $"{this.Name}: {this.Id}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Player other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    internal void AddGameToHistory(GamePair pair, GamePair.PlayerColor color)
    {
        if (this.TryAddGamePair(pair, color))
        {
            return;
        }

        this.RemoveGamePair(pair, color);
        if (!this.TryAddGamePair(pair, color))
        {
            throw new DomainException($"Failed to apply new game result: {pair}.");
        }
    }

    private bool TryAddGamePair(GamePair pair, GamePair.PlayerColor color)
    {
        if (!this._gamesHistory.Add(pair))
        {
            return false;
        }

        switch (pair.Result)
        {
            // Win.
            case GamePair.GameResult.WhiteWinByDefault when color == GamePair.PlayerColor.White:
            case GamePair.GameResult.WhiteWin when color          == GamePair.PlayerColor.White:
            case GamePair.GameResult.BlackWinByDefault when color == GamePair.PlayerColor.Black:
            case GamePair.GameResult.BlackWin when color          == GamePair.PlayerColor.Black:
                this.Points += 1;
                this.Wins   += 1;

                break;
            // Draw.
            case GamePair.GameResult.Draw:
                this.Points += 0.5m;
                this.Draws  += 1;

                break;
            // Lose.
            case GamePair.GameResult.WhiteWinByDefault:
            case GamePair.GameResult.WhiteWin:
            case GamePair.GameResult.BlackWinByDefault:
            case GamePair.GameResult.BlackWin:
            // Both leave.
            case GamePair.GameResult.NotPlayed:
                this.Loses += 1;

                break;
            default:
                throw new DomainOutOfRangeException(nameof(pair.Result), pair.Result);
        }

        return true;
    }

    private void RemoveGamePair(GamePair pair, GamePair.PlayerColor color)
    {
        if (!this._gamesHistory.Remove(pair))
        {
            throw new DomainException($"Failed to remove game result: {pair}, because it is not found.");
        }

        switch (pair.Result)
        {
            // Was win.
            case GamePair.GameResult.WhiteWinByDefault when color == GamePair.PlayerColor.White:
            case GamePair.GameResult.WhiteWin when color          == GamePair.PlayerColor.White:
            case GamePair.GameResult.BlackWinByDefault when color == GamePair.PlayerColor.Black:
            case GamePair.GameResult.BlackWin when color          == GamePair.PlayerColor.Black:
                this.Points -= 1;
                this.Wins   -= 1;

                break;
            // Was draw.
            case GamePair.GameResult.Draw:
                this.Points -= 0.5m;
                this.Draws  -= 1;

                break;
            // Was lose.
            case GamePair.GameResult.WhiteWinByDefault:
            case GamePair.GameResult.WhiteWin:
            case GamePair.GameResult.BlackWinByDefault:
            case GamePair.GameResult.BlackWin:
            // Was both leave.
            case GamePair.GameResult.NotPlayed:
                this.Loses -= 1;

                break;
            default:
                throw new DomainOutOfRangeException(nameof(pair.Result), pair.Result);
        }
    }
}
