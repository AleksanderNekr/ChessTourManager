using System.Numerics;
using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public readonly struct TourNumber : IMinMaxValue<int>, IEquatable<TourNumber>, IComparable<TourNumber>
{
	private readonly int _value;

	public static int MinValue => 1;

	public static int MaxValue => 20;

	public static TourNumber BeforeStart()
		=> new();

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
		=> tourNumber._value;

	public static implicit operator TourNumber(in int tourNumber)
		=> new(in tourNumber);

	public override string ToString()
		=> _value.ToString();

	public override int GetHashCode()
		=> _value;

	public static bool operator ==(in TourNumber left, in TourNumber right)
		=> left.Equals(right);

	public static bool operator !=(in TourNumber left, in TourNumber right)
		=> !left.Equals(right);

	public static bool operator <(in TourNumber left, in TourNumber right)
		=> left.CompareTo(right) < 0;

	public static bool operator >(in TourNumber left, in TourNumber right)
		=> left.CompareTo(right) > 0;

	public static bool operator <=(in TourNumber left, in TourNumber right)
		=> left.CompareTo(right) <= 0;

	public static bool operator >=(in TourNumber left, in TourNumber right)
		=> left.CompareTo(right) >= 0;

	public override bool Equals(object? obj)
		=> obj is TourNumber other && Equals(other);

	public bool Equals(TourNumber other)
		=> _value == other._value;

	public int CompareTo(TourNumber other)
		=> _value.CompareTo(other._value);
}