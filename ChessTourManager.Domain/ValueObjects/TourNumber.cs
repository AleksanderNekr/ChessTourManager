﻿using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public readonly struct TourNumber
{
    private readonly int _value;
    private const    int Min = 1;
    private const    int Max = 20;

    public TourNumber(in int value)
    {
        if (value is < Min or > Max)
        {
            throw new DomainException($"Tour number must be between {Min} and {Max}");
        }

        this._value = value;
    }

    public TourNumber NextTourNumber()
    {
        if (this._value is Max)
        {
            throw new DomainException("Max possible tour number reached");
        }

        return this._value + 1;
    }

    public TourNumber PreviousTourNumber()
    {
        if (this._value is Min)
        {
            throw new DomainException("Min possible tour number reached");
        }

        return this._value - 1;
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
        return this._value.ToString();
    }

    public override int GetHashCode()
    {
        return this._value;
    }

    public static bool operator ==(in TourNumber left, in TourNumber right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(in TourNumber left, in TourNumber right)
    {
        return !(left == right);
    }

    public static bool operator <(in TourNumber left, in TourNumber right)
    {
        return left._value < right._value;
    }

    public static bool operator >(in TourNumber left, in TourNumber right)
    {
        return left._value > right._value;
    }

    public static bool operator <=(in TourNumber left, in TourNumber right)
    {
        return left._value <= right._value;
    }

    public static bool operator >=(in TourNumber left, in TourNumber right)
    {
        return left._value >= right._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is TourNumber other && this.Equals(other);
    }

    public bool Equals(TourNumber other)
    {
        return this._value == other._value;
    }

    public int CompareTo(TourNumber other)
    {
        return this._value.CompareTo(other._value);
    }
}
