namespace ChessTourManager.Domain.Interfaces.Tests;

public class ByNameComparerTests
{
    public class NameableTests
    {
        [Fact]
        public void ByNameEqualityComparer_EqualNames_ReturnsTrue()
        {
            // Arrange
            var name1 = new Name("John");
            var name2 = new Name("John");

            // Act
            var  comparer = new INameable.ByNameEqualityComparer<INameable>();
            bool result   = comparer.Equals(new TestNameable(name1), new TestNameable(name2));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ByNameEqualityComparer_DifferentNames_ReturnsFalse()
        {
            // Arrange
            var name1 = new Name("John");
            var name2 = new Name("Jane");

            // Act
            var  comparer = new INameable.ByNameEqualityComparer<INameable>();
            bool result   = comparer.Equals(new TestNameable(name1), new TestNameable(name2));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ByNameEqualityComparer_NullObjects_ReturnsFalse()
        {
            // Arrange
            var name = new Name("John");

            // Act
            var  comparer = new INameable.ByNameEqualityComparer<INameable>();
            bool result   = comparer.Equals(null, new TestNameable(name));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ByNameEqualityComparer_SameObject_ReturnsTrue()
        {
            // Arrange
            var name         = new Name("John");
            var testNameable = new TestNameable(name);

            // Act
            var  comparer = new INameable.ByNameEqualityComparer<INameable>();
            bool result   = comparer.Equals(testNameable, testNameable);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ByNameEqualityComparer_GetHashCode_ReturnsSameValueForEqualNames()
        {
            // Arrange
            var name1 = new Name("John");
            var name2 = new Name("John");

            // Act
            var comparer  = new INameable.ByNameEqualityComparer<INameable>();
            int hashCode1 = comparer.GetHashCode(new TestNameable(name1));
            int hashCode2 = comparer.GetHashCode(new TestNameable(name2));

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void ByNameEqualityComparer_GetHashCode_ReturnsDifferentValueForDifferentNames()
        {
            // Arrange
            var name1 = new Name("John");
            var name2 = new Name("Jane");

            // Act
            var comparer  = new INameable.ByNameEqualityComparer<INameable>();
            int hashCode1 = comparer.GetHashCode(new TestNameable(name1));
            int hashCode2 = comparer.GetHashCode(new TestNameable(name2));

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }

    // Helper class for testing the INameable interface
    private class TestNameable : INameable
    {
        public TestNameable(Name name)
        {
            this.Name = name;
        }

        public Name Name { get; }
    }
}
