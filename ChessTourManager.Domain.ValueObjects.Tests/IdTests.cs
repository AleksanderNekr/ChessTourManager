namespace ChessTourManager.Domain.ValueObjects.Tests;

public class IdTests
{
    [Fact]
    public void TestImplicitOperator_FromIdToInnerType()
    {
        // Arrange
        Id<Guid> id = new(Guid.NewGuid());

        // Act
        Guid guid = id;

        // Assert
        Assert.Equal((Guid)id, guid);
    }

    [Fact]
    public void TestImplicitOperator_FromInnerTypeToId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id = guid;

        // Assert
        Assert.Equal(guid, (Guid)id);
    }

    [Fact]
    public void TestEqualsOperator_Correct()
    {
        // Arrange
        var guid1 = Guid.NewGuid();

        // Act
        Id<Guid> id1 = guid1;
        Id<Guid> id2 = guid1;

        // Assert
        Assert.True(id1 == id2);
    }

    [Fact]
    public void TestEqualsOperator_Incorrect()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        // Act
        Id<Guid> id1 = guid1;
        Id<Guid> id2 = guid2;

        // Assert
        Assert.False(id1 == id2);
    }

    [Fact]
    public void TestUnequalsOperator_Correct()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        // Act
        Id<Guid> id1 = guid1;
        Id<Guid> id2 = guid2;

        // Assert
        Assert.True(id1 != id2);
    }

    [Fact]
    public void TestUnequalsOperator_Incorrect()
    {
        // Arrange
        var guid1 = Guid.NewGuid();

        // Act
        Id<Guid> id1 = guid1;
        Id<Guid> id2 = guid1;

        // Assert
        Assert.False(id1 != id2);
    }

    [Fact]
    public void TestToString()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id = guid;

        // Assert
        Assert.Equal(guid.ToString(), id.ToString());
    }

    [Fact]
    public void TestEquals()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id1 = guid;
        Id<Guid> id2 = guid;

        // Assert
        Assert.True(id1.Equals(id2));
    }

    [Fact]
    public void TestGetHashCode()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id = guid;

        // Assert
        Assert.Equal(guid.GetHashCode(), id.GetHashCode());
    }

    [Fact]
    public void TestEquals_Null()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id = guid;

        // Assert
        Assert.False(id.Equals(null));
    }

    [Fact]
    public void TestEquals_WrongType()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id = guid;

        // Assert
        Assert.False(id.Equals(new object()));
    }

    [Fact]
    public void TestEquals_CorrectType()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Id<Guid> id = guid;

        // Assert
        Assert.True(id.Equals((object)id));
    }
}
