using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class Participant<TParticipant> where TParticipant : Participant<TParticipant>
{
    private decimal _points;

    private readonly HashSet<GamePair<TParticipant>> _gamesHistory;

    protected Participant(Id<Guid> id, Name name, bool isActive)
    {
        this.Id       = id;
        this.Name     = name;
        this.IsActive = isActive;
        this._gamesHistory =
            new HashSet<GamePair<TParticipant>>(comparer: new GamePair<TParticipant>.ByPlayersEqualityComparer());
    }

    internal Id<Guid> Id { get; }

    public Name Name { get; }

    protected decimal Points
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

    protected uint Wins { get; private set; }

    protected uint Draws { get; private set; }

    protected uint Loses { get; private set; }

    public bool IsActive { get; private set; }

    public IEnumerable<TParticipant> GetAllOpponents()
    {
        foreach (GamePair<TParticipant> pair in this._gamesHistory)
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

    public IEnumerable<TParticipant> GetBlackOpponents()
    {
        return this._gamesHistory
                   .Where(pair => pair.White.Equals(this))
                   .Select(static pair => pair.Black);
    }

    public IEnumerable<TParticipant> GetWhiteOpponents()
    {
        return this._gamesHistory
                   .Where(pair => pair.Black.Equals(this))
                   .Select(static pair => pair.White);
    }

    internal virtual void SetActive()
    {
        this.IsActive = true;
    }

    internal virtual void SetInactive()
    {
        this.IsActive = false;
    }

    internal bool InGames()
    {
        return this._gamesHistory.Any();
    }

    internal void AddGameToHistory(GamePair<TParticipant> pair, PlayerColor color)
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

    private protected virtual bool TryAddGamePair(GamePair<TParticipant> pair, PlayerColor color)
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

    private protected virtual void RemoveGamePair(GamePair<TParticipant> pair, PlayerColor color)
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
