using System.Numerics;
using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public readonly struct TourNumber : IMinMaxValue<int>, IEquatable<TourNumber>, IComparable<TourNumber>
{
    private readonly int _value;

    public static int MinValue
    {
        get => 1;
    }

    public static int MaxValue
    {
        get => 20;
    }

    public TourNumber(in int value)
    {
        if (value < MinValue || value > MaxValue)
        {
            throw new DomainException($"Tour number must be between {MinValue} and {MaxValue}");
        }

        _value = value;
    }

    public TourNumber NextTourNumber()
    {
        if (_value == MaxValue)
        {
            throw new DomainException("Max possible tour number reached");
        }

        return _value + 1;
    }

    public TourNumber PreviousTourNumber()
    {
        if (_value == MinValue)
        {
            throw new DomainException("Min possible tour number reached");
        }

        return _value - 1;
    }

    public static implicit operator int(in TourNumber tourNumber)
    {
        return tourNumber._value;
    }

    public static implicit operator TourNumber(in int tourNumber)
    {
        return new TourNumber(in tourNumber);
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public override int GetHashCode()
    {
        return _value;
    }

    public static bool operator ==(in TourNumber left, in TourNumber right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(in TourNumber left, in TourNumber right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(in TourNumber left, in TourNumber right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(in TourNumber left, in TourNumber right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(in TourNumber left, in TourNumber right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(in TourNumber left, in TourNumber right)
    {
        return left.CompareTo(right) >= 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is TourNumber other && Equals(other);
    }

    public bool Equals(TourNumber other)
    {
        return _value == other._value;
    }

    public int CompareTo(TourNumber other)
    {
        return _value.CompareTo(other._value);
    }
}
