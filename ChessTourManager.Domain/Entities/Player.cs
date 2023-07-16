using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Player : IEquatable<Player>, INameable, IPlayer<Player>
{
    private readonly HashSet<GamePair<Player>> _gamesHistory;
    private          decimal                   _points;

    public Player(Id<Guid> id, Name name)
    {
        this.Id            = id;
        this.Name          = name;
        this.IsActive      = true;
        this._gamesHistory = new HashSet<GamePair<Player>>(comparer: new GamePair<Player>.ByPlayersEqualityComparer());
    }

    private Id<Guid> Id { get; }

    public Name Name { get; }

    public decimal Points
    {
        get => this._points;
        private set
        {
            if (value < 0)
            {
                throw new DomainException("Points cannot be negative.");
            }

            this._points = value;
        }
    }

    public uint Wins { get; private set; }

    public uint Draws { get; private set; }

    public uint Loses { get; private set; }

    public bool IsActive { get; private set; }

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
        foreach (GamePair<Player> pair in this._gamesHistory)
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

    public void AddGameToHistory(GamePair<Player> pair, PlayerColor color)
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

    public void SetActive()
    {
        this.IsActive = true;
    }

    public void SetInactive()
    {
        this.IsActive = false;
    }

    private bool TryAddGamePair(GamePair<Player> pair, PlayerColor color)
    {
        if (!this._gamesHistory.Add(pair))
        {
            return false;
        }

        switch (pair.Result)
        {
            // Win.
            case GameResult.WhiteWinByDefault when color == PlayerColor.White:
            case GameResult.WhiteWin when color          == PlayerColor.White:
            case GameResult.BlackWinByDefault when color == PlayerColor.Black:
            case GameResult.BlackWin when color          == PlayerColor.Black:
                this.Points += 1;
                this.Wins   += 1;

                break;
            // Draw.
            case GameResult.Draw:
                this.Points += 0.5m;
                this.Draws  += 1;

                break;
            // Lose.
            case GameResult.WhiteWinByDefault:
            case GameResult.WhiteWin:
            case GameResult.BlackWinByDefault:
            case GameResult.BlackWin:
            // Both leave.
            case GameResult.BothLeave:
                this.Loses += 1;

                break;
            case GameResult.NotYetPlayed:
                break;
            default:
                throw new DomainOutOfRangeException(nameof(pair.Result), pair.Result);
        }

        return true;
    }

    private void RemoveGamePair(GamePair<Player> pair, PlayerColor color)
    {
        if (!this._gamesHistory.Remove(pair))
        {
            throw new DomainException($"Failed to remove game result: {pair}, because it is not found.");
        }

        switch (pair.Result)
        {
            // Was win.
            case GameResult.WhiteWinByDefault when color == PlayerColor.White:
            case GameResult.WhiteWin when color          == PlayerColor.White:
            case GameResult.BlackWinByDefault when color == PlayerColor.Black:
            case GameResult.BlackWin when color          == PlayerColor.Black:
                this.Points -= 1;
                this.Wins   -= 1;

                break;
            // Was draw.
            case GameResult.Draw:
                this.Points -= 0.5m;
                this.Draws  -= 1;

                break;
            // Was lose.
            case GameResult.WhiteWinByDefault:
            case GameResult.WhiteWin:
            case GameResult.BlackWinByDefault:
            case GameResult.BlackWin:
            // Was both leave.
            case GameResult.BothLeave:
                this.Loses -= 1;

                break;
            case GameResult.NotYetPlayed:
                break;
            default:
                throw new DomainOutOfRangeException(nameof(pair.Result), pair.Result);
        }
    }
}
