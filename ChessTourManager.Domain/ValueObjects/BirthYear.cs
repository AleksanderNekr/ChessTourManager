using System.Numerics;
using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public readonly struct BirthYear : IMinMaxValue<int>, IEquatable<BirthYear>, IComparable<BirthYear>
{
	private readonly int _value = MaxValue;

	private const int OldestAge = 150;

	public static int MinValue => DateTime.UtcNow.Year - OldestAge;

	public static int MaxValue => DateTime.UtcNow.Year;

	private BirthYear(in int value)
	{
		if (value < MinValue || value > MaxValue)
		{
			throw new DomainException($"Birth year must be between {MinValue} and {MaxValue}");
		}

		_value = value;
	}

	public static implicit operator int(in BirthYear tourNumber)
		=> tourNumber._value;

	public static implicit operator BirthYear(in int tourNumber)
		=> new(in tourNumber);

	public override string ToString()
		=> _value.ToString();

	public override int GetHashCode()
		=> _value;

	public static bool operator ==(in BirthYear left, in BirthYear right)
		=> left.Equals(right);

	public static bool operator !=(in BirthYear left, in BirthYear right)
		=> !left.Equals(right);

	public static bool operator <(in BirthYear left, in BirthYear right)
		=> left.CompareTo(right) < 0;

	public static bool operator >(in BirthYear left, in BirthYear right)
		=> left.CompareTo(right) > 0;

	public static bool operator <=(in BirthYear left, in BirthYear right)
		=> left.CompareTo(right) <= 0;

	public static bool operator >=(in BirthYear left, in BirthYear right)
		=> left.CompareTo(right) >= 0;

	public override bool Equals(object? obj)
		=> obj is BirthYear other && Equals(other);

	public bool Equals(BirthYear other)
		=> _value == other._value;

	public int CompareTo(BirthYear other)
		=> _value.CompareTo(other._value);
}