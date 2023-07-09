namespace ChessTourManager.Domain.ValueObjects;

public readonly struct Id
{
    public bool Equals(Id other)
    {
        return this._value.Equals(other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && this.Equals((Id)obj);
    }

    public override int GetHashCode()
    {
        return this._value.GetHashCode();
    }

    private readonly Guid _value;

    public Id(in Guid value)
    {
        this._value = value;
    }

    public static implicit operator Guid(in Id id)
    {
        return id._value;
    }

    public static implicit operator Id(in Guid guid)
    {
        return new Id(in guid);
    }

    public static bool operator ==(in Id left, in Id right)
    {
        return left._value == right._value;
    }

    public static bool operator !=(Id left, Id right)
    {
        return !(left == right);
    }

    public static bool operator <(in Id left, in Id right)
    {
        return left._value < right._value;
    }

    public static bool operator >(in Id left, in Id right)
    {
        return left._value > right._value;
    }

    public static bool operator <=(in Id left, in Id right)
    {
        return left._value <= right._value;
    }

    public static bool operator >=(in Id left, in Id right)
    {
        return left._value >= right._value;
    }

    public int CompareTo(Id other)
    {
        return this._value.CompareTo(other._value);
    }
}
