using System.Numerics;
using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public readonly struct BirthYear : IMinMaxValue<int>, IEquatable<BirthYear>, IComparable<BirthYear>
{
    private readonly int _value = MaxValue;

    private const int OldestAge = 150;

    public static int MinValue
    {
        get => DateTime.UtcNow.Year - OldestAge;
    }

    public static int MaxValue
    {
        get => DateTime.UtcNow.Year;
    }

    private BirthYear(in int value)
    {
        if (value < MinValue || value > MaxValue)
        {
            throw new DomainException($"Birth year must be between {MinValue} and {MaxValue}");
        }

        _value = value;
    }

    public static implicit operator int(in BirthYear tourNumber)
    {
        return tourNumber._value;
    }

    public static implicit operator BirthYear(in int tourNumber)
    {
        return new BirthYear(in tourNumber);
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public override int GetHashCode()
    {
        return _value;
    }

    public static bool operator ==(in BirthYear left, in BirthYear right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(in BirthYear left, in BirthYear right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(in BirthYear left, in BirthYear right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(in BirthYear left, in BirthYear right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(in BirthYear left, in BirthYear right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(in BirthYear left, in BirthYear right)
    {
        return left.CompareTo(right) >= 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is BirthYear other && Equals(other);
    }

    public bool Equals(BirthYear other)
    {
        return _value == other._value;
    }

    public int CompareTo(BirthYear other)
    {
        return _value.CompareTo(other._value);
    }
}
