using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Group : IEquatable<Group>, INameable
{
    private readonly HashSet<Player> _players;

    internal Group(Id<Guid> id, Name name, HashSet<Player>? players = null)
    {
        this.Id   = id;
        this.Name = name;
        this._players = players ?? new HashSet<Player>(new INameable.ByNameEqualityComparer<Player>());
    }

    public Id<Guid> Id { get; }

    internal IReadOnlySet<Player> Players
    {
        get => this._players;
    }

    public Name Name { get; set; }

    public bool Equals(Group? other)
    {
        return other is not null
            && this.Id   == other.Id
            && this.Name == other.Name
            && this.Players.SequenceEqual(other.Players);
    }

    public override bool Equals(object? obj)
    {
        return obj is Group group && this.Equals(group);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    internal AddPlayerResult TryAddPlayer(Id<Guid> playerId, Name playerName, Gender gender, BirthYear birthYear)
    {
        bool isUnique = this._players.Add(new Player(playerId, playerName, gender, birthYear));

        return isUnique
                   ? AddPlayerResult.Success
                   : AddPlayerResult.PlayerAlreadyExists;
    }

    internal RemovePlayerResult TryRemovePlayer(Id<Guid> playerId)
    {
        Player? player = this._players.SingleOrDefault(player => player.Id == playerId);
        if (player is null)
        {
            return RemovePlayerResult.PlayerDoesNotExist;
        }

        if (player.InGames())
        {
            return RemovePlayerResult.PlayerInGames;
        }

        bool isRemoved = this._players.Remove(player);

        return isRemoved
                   ? RemovePlayerResult.Success
                   : RemovePlayerResult.Fail;
    }
}
