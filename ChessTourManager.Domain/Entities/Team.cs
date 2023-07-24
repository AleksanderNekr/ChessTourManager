using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Team : Participant<Team>, IEquatable<Team>, INameable
{
    private readonly LinkedList<Player> _players;

    internal Team(Id<Guid> id, Name name, bool isActive = true, IEnumerable<Player>? players = default)
        : base(id, name, isActive)
    {
        this._players = new LinkedList<Player>(players ?? Enumerable.Empty<Player>());
    }

    internal IEnumerable<Player> Players
    {
        get => this._players;
    }

    internal override void SetActive()
    {
        base.SetActive();
        foreach (Player player in this._players)
        {
            player.SetActive();
        }
    }

    internal override void SetInactive()
    {
        base.SetInactive();
        foreach (Player player in this._players)
        {
            player.SetInactive();
        }
    }

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
}
