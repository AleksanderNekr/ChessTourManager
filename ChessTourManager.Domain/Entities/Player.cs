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
        this._gamesHistory = new HashSet<GamePair>();
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
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        return ReferenceEquals(this, other) || this.Id.Equals(other.Id) && this.Name.Equals(other.Name);
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

    internal void AddGameToHistory(GamePair pair)
    {
        PlayerColor color = this.GetColorInPair(pair);

        // If game is not in history, add it.
        if (this.TryApplyGameResult(pair, color))
        {
            return;
        }

        // If game is already in history, remove it.
        // Then apply new result instead.
        this.RemoveGameResult(pair, color);
        if (!this.TryApplyGameResult(pair, color))
        {
            throw new DomainException($"Failed to apply new game result: {pair}.");
        }
    }

    private PlayerColor GetColorInPair(GamePair pair)
    {
        return pair.White.Equals(this)
                   ? PlayerColor.White
                   : PlayerColor.Black;
    }

    private bool TryApplyGameResult(GamePair pair, PlayerColor color)
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
            case GameResult.NotPlayed:
                this.Loses += 1;

                break;
            default:
                throw new DomainOutOfRangeException(nameof(pair.Result), pair.Result);
        }

        return true;
    }

    private void RemoveGameResult(GamePair pair, PlayerColor color)
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
            case GameResult.NotPlayed:
                this.Loses -= 1;

                break;
            default:
                throw new DomainOutOfRangeException(nameof(pair.Result), pair.Result);
        }
    }
}
