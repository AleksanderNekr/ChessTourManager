using System.Text.RegularExpressions;
using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public sealed class Name : IEquatable<Name>
{
    private readonly string _value;

    public Name(string value)
    {
        value = Regex.Replace(value.Trim(), @"\s+", " ");
        if (value.Length is < 2 or > 50)
        {
            throw new DomainException("Name must be between 2 and 50 characters");
        }

        this._value = value;
    }

    public static implicit operator string(Name name)
    {
        return name._value;
    }

    public static implicit operator Name(string name)
    {
        return new Name(name);
    }

    public override string ToString()
    {
        return this._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Name other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this._value.GetHashCode();
    }

    public static bool operator ==(Name left, Name right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Name left, Name right)
    {
        return !(left == right);
    }

    public bool Equals(Name? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this._value == other._value;
    }
}
