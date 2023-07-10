namespace ChessTourManager.Domain.ValueObjects;

public readonly struct Id<TId> where TId : IComparable<TId>
{
    public bool Equals(in Id<TId> other)
    {
        return this == other;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && this.Equals((Id<TId>)obj);
    }

    public override int GetHashCode()
    {
        return this._value.GetHashCode();
    }

    private readonly TId _value;

    public Id(in TId value)
    {
        this._value = value;
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
        return !(left == right);
    }

    public int CompareTo(in Id<TId> other)
    {
        return this._value.CompareTo(other._value);
    }

    public override string ToString()
    {
        return (this._value.ToString() ?? base.ToString()) ?? string.Empty;
    }
}
