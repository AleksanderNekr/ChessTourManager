namespace ChessTourManager.Domain.ValueObjects;

public readonly struct Id<TId> : IEquatable<Id<TId>>, IComparable<Id<TId>>
    where TId : IEquatable<TId>, IComparable<TId>
{
    public bool Equals(Id<TId> other)
    {
        return CompareTo(other) == 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is Id<TId> id && Equals(id);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    private readonly TId _value;

    public Id(in TId value)
    {
        _value = value;
    }

    public static implicit operator TId(in Id<TId> id)
    {
        return id._value;
    }

    public static implicit operator Id<TId>(in TId guid)
    {
        return new Id<TId>(in guid);
    }

    public static bool operator ==(in Id<TId> left, in Id<TId> right)
    {
        return left.CompareTo(right) == 0;
    }

    public static bool operator !=(in Id<TId> left, in Id<TId> right)
    {
        return left.CompareTo(right) != 0;
    }

    public int CompareTo(Id<TId> other)
    {
        return _value.CompareTo(other._value);
    }

    public override string ToString()
    {
        return (_value.ToString() ?? base.ToString()) ?? string.Empty;
    }
}
