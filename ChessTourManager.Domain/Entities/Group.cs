using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Group : IEquatable<Group>, INameable
{
    public Group(Id<Guid> id, Name name, IEnumerable<Player> players)
    {
        this.Id      = id;
        this.Name    = name;
        this.Players = players;
    }

    public Id<Guid> Id { get; }

    public IEnumerable<Player> Players { get; set; }

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
}
