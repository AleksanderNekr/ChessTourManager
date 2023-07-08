using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public class Name
{
    private readonly string _value;

    public Name(string value)
    {
        value = Regex.Replace(value, @"\s+", " ");
        ReadOnlySpan<char> trimmed = value.AsSpan().Trim();
        if (trimmed.Length is < 2 or > 50)
        {
            throw new ArgumentException("Name must be between 2 and 50 characters");
        }

        this._value = trimmed.ToString();
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
        if (obj is not Name other)
        {
            return false;
        }

        return this._value == other._value;
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
}
