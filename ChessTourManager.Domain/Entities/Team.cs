using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Team : IEquatable<Team>
{
    public Id<Guid> Id { get; }

    public IEnumerable<Player> Players { get; set; }

    public Name Name { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Team team && this.Equals(team);
    }

    public bool Equals(Team? other)
    {
        return other is not null && this.Id == other.Id && this.Name == other.Name;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}
