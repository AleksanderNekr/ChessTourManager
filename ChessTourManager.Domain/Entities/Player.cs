using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Player : Participant<Player>, IEquatable<Player>, INameable
{
    internal Player(Id<Guid> id, Name name, Gender gender, BirthYear birthYear, bool isActive = true)
        : base(id, name, isActive)
    {
        PlayerGender = gender;
        BirthYear    = birthYear;
    }

    public Gender PlayerGender { get; set; }

    public BirthYear BirthYear { get; set; }

    public bool Equals(Player? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id     == other.Id
            && Name   == other.Name
            && Points == other.Points
            && Wins   == other.Wins
            && Draws  == other.Draws
            && Loses  == other.Loses;
    }


    public override string ToString()
    {
        return $"{Name}: {Id}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Player other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public enum Gender
{
    Male,
    Female,
}
