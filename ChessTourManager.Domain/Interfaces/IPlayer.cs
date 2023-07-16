using ChessTourManager.Domain.Entities;

namespace ChessTourManager.Domain.Interfaces;

public interface IPlayer<TPlayer> where TPlayer : IPlayer<TPlayer>
{
    decimal Points { get; }

    uint Wins { get; }

    uint Draws { get; }

    uint Loses { get; }

    bool IsActive { get; }

    IEnumerable<TPlayer> GetAllOpponents();

    IEnumerable<TPlayer> GetBlackOpponents();

    IEnumerable<TPlayer> GetWhiteOpponents();

    internal void AddGameToHistory(GamePair<TPlayer> pair, PlayerColor color);

    public void SetActive();

    public void SetInactive();
}
