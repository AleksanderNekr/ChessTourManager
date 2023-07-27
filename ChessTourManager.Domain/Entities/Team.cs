using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Team : Participant<Team>, IEquatable<Team>, INameable
{
    private readonly LinkedList<Player> _players;

    internal Team(Id<Guid> id, Name name, bool isActive = true, IEnumerable<Player>? players = default)
        : base(id, name, isActive)
    {
        _players = new LinkedList<Player>(players ?? Enumerable.Empty<Player>());
    }

    internal IEnumerable<Player> Players
    {
        get => _players;
    }

    public bool Equals(Team? other)
    {
        return other is not null
            && Id   == other.Id
            && Name == other.Name
            && Players.SequenceEqual(other.Players);
    }

    internal override void SetActive()
    {
        base.SetActive();
        foreach (Player player in _players)
        {
            player.SetActive();
        }
    }

    internal override void SetInactive()
    {
        base.SetInactive();
        foreach (Player player in _players)
        {
            player.SetInactive();
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Team team && Equals(team);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
