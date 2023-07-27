using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ChessTourManager.Domain.Benchmarks;

public class NameWithoutRef
{
    private readonly string _value;

    public NameWithoutRef(string value)
    {
        value = Regex.Replace(value, @"\s+", " ");
        ReadOnlySpan<char> trimmed = value.AsSpan().Trim();
        if (trimmed.Length is < 2 or > 50)
        {
            throw new Exception("NameWithRef must be between 2 and 50 characters");
        }

        _value = trimmed.ToString();
    }

    public static implicit operator string(NameWithoutRef name)
    {
        return name._value;
    }

    public static implicit operator NameWithoutRef(string name)
    {
        return new NameWithoutRef(name);
    }

    public override string ToString()
    {
        return _value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not NameWithoutRef other)
        {
            return false;
        }

        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(NameWithoutRef left, NameWithoutRef right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NameWithoutRef left, NameWithoutRef right)
    {
        return !(left == right);
    }
}

public class NameWithRef
{
    private readonly string _value;

    public NameWithRef(ref string value)
    {
        value = Regex.Replace(value, @"\s+", " ");
        ReadOnlySpan<char> trimmed = value.AsSpan().Trim();
        if (trimmed.Length is < 2 or > 50)
        {
            throw new Exception("NameWithRef must be between 2 and 50 characters");
        }

        _value = trimmed.ToString();
    }

    public static implicit operator string(NameWithRef name)
    {
        return name._value;
    }

    public static implicit operator NameWithRef(string name)
    {
        return new NameWithRef(ref name);
    }

    public override string ToString()
    {
        return _value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not NameWithRef other)
        {
            return false;
        }

        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(NameWithRef left, NameWithRef right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NameWithRef left, NameWithRef right)
    {
        return !(left == right);
    }
}

[MemoryDiagnoser]
public class NameBenchmarks
{
    [Params("John Doe", "   Alex   Smith   ", "  John  \t Smith   ")]
    public string Name;

    [Benchmark]
    public void NameWithoutRef()
    {
        var name = new NameWithoutRef(Name);

        _ = name.ToString();
    }

    [Benchmark]
    public void NameWithRef()
    {
        var name = new NameWithRef(ref Name);

        _ = name.ToString();
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<NameBenchmarks>();
    }
}
