using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public sealed class Player : Participant<Player>, IEquatable<Player>, INameable
{
    internal Player(Id<Guid> id, Name name, Gender gender, BirthYear birthYear, bool isActive = true)
        : base(id, name, isActive)
    {
        this.PlayerGender = gender;
        this.BirthYear    = birthYear;
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

        return this.Id     == other.Id
            && this.Name   == other.Name
            && this.Points == other.Points
            && this.Wins   == other.Wins
            && this.Draws  == other.Draws
            && this.Loses  == other.Loses;
    }


    public override string ToString()
    {
        return $"{this.Name}: {this.Id}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Player other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}

public enum Gender
{
    Male,
    Female,
}
