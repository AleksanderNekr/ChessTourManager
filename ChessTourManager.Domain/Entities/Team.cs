using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Team : IEquatable<Team>
{
    public Team(Id<Guid> id, Name name, IEnumerable<Player> players)
    {
        this.Id      = id;
        this.Players = players;
        this.Name    = name;
    }

    public Id<Guid> Id { get; }

    public IEnumerable<Player> Players { get; set; }

    public Name Name { get; set; }

    public bool Equals(Team? other)
    {
        return other is not null && this.Id   == other.Id
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
