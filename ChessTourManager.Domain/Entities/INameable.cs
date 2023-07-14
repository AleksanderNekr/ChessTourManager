using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

internal interface INameable
{
    public Name Name { get; }

    internal sealed class ByNameEqualityComparer<T> : IEqualityComparer<T> where T : INameable
    {
        public bool Equals(T? x, T? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(T obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
