using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Team : IEquatable<Team>, INameable, IPlayer<Team>
{
    private readonly HashSet<GamePair<Team>> _gamesHistory;

    private decimal _points;

    public Team(Id<Guid> id, Name name, IEnumerable<Player> players)
    {
        this.Id            = id;
        this.Players       = players;
        this.Name          = name;
        this.IsActive      = true;
        this._gamesHistory = new HashSet<GamePair<Team>>(new GamePair<Team>.ByPlayersEqualityComparer());
    }

    public Id<Guid> Id { get; }

    public IEnumerable<Player> Players { get; set; }

    public Name Name { get; set; }

    public bool Equals(Team? other)
    {
        return other is not null
            && this.Id   == other.Id
            && this.Name == other.Name
            && this.Players.SequenceEqual(other.Players);
    }

    public override bool Equals(object? obj)
    {
        return obj is Team team && this.Equals(team);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

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

    public IEnumerable<Team> GetAllOpponents()
    {
        foreach (GamePair<Team> pair in this._gamesHistory)
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

    public IEnumerable<Team> GetBlackOpponents()
    {
        return this._gamesHistory
                   .Where(pair => pair.White.Equals(this))
                   .Select(static pair => pair.Black);
    }

    public IEnumerable<Team> GetWhiteOpponents()
    {
        return this._gamesHistory
                   .Where(pair => pair.Black.Equals(this))
                   .Select(static pair => pair.White);
    }

    public void AddGameToHistory(GamePair<Team> pair, PlayerColor color)
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

    private bool TryAddGamePair(GamePair<Team> pair, PlayerColor color)
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

    private void RemoveGamePair(GamePair<Team> pair, PlayerColor color)
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
