using System.Text.RegularExpressions;
using ChessTourManager.Domain.Exceptions;

namespace ChessTourManager.Domain.ValueObjects;

public sealed class Name : IEquatable<Name>, IComparable<Name>
{
	private readonly string _value;

	public Name(string value)
	{
		value = Regex.Replace(value.Trim(), @"\s+", " ");
		if (value.Length is < 2 or > 50)
		{
			throw new DomainException("Name must be between 2 and 50 characters");
		}

		_value = value;
	}

	public int CompareTo(Name? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		return string.Compare(_value, other._value, StringComparison.Ordinal);
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

		return _value == other._value;
	}

	public static implicit operator string(Name name)
		=> name._value;

	public static implicit operator Name(string name)
		=> new(name);

	public override string ToString()
		=> _value;

	public override bool Equals(object? obj)
		=> obj is Name other && Equals(other);

	public override int GetHashCode()
		=> _value.GetHashCode();

	public static bool operator ==(Name left, Name right)
		=> left.Equals(right);

	public static bool operator !=(Name left, Name right)
		=> !(left == right);

	public static bool operator <(Name? left, Name? right)
		=> Comparer<Name>.Default.Compare(left, right) < 0;

	public static bool operator >(Name? left, Name? right)
		=> Comparer<Name>.Default.Compare(left, right) > 0;

	public static bool operator <=(Name? left, Name? right)
		=> Comparer<Name>.Default.Compare(left, right) <= 0;

	public static bool operator >=(Name? left, Name? right)
		=> Comparer<Name>.Default.Compare(left, right) >= 0;
}