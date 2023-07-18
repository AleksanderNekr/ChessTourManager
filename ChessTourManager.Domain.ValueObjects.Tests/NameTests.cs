namespace ChessTourManager.Domain.ValueObjects.Tests;

public class NameTests
{
    [Fact]
    public void TestImplicitOperator_FromNameToInnerType()
    {
        // Arrange
        Name name = new("Name");

        // Act
        string str = name;

        // Assert
        Assert.Equal((string)name, str);
    }

    [Fact]
    public void TestImplicitOperator_FromInnerTypeToName()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name = str;

        // Assert
        Assert.Equal(str, (string)name);
    }

    [Fact]
    public void TestEqualsOperator_Correct()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;
        Name name2 = str;

        // Assert
        Assert.True(name1 == name2);
    }

    [Fact]
    public void TestEqualsOperator_Incorrect()
    {
        // Arrange
        const string str1 = "Name1";
        const string str2 = "Name2";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.False(name1 == name2);
    }

    [Fact]
    public void TestUnequalsOperator_Correct()
    {
        // Arrange
        const string str1 = "Name1";
        const string str2 = "Name2";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 != name2);
    }


    [Fact]
    public void TestUnequalsOperator_Incorrect()
    {
        // Arrange
        const string str1 = "Name1";
        const string str2 = "Name1";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.False(name1 != name2);
    }

    [Fact]
    public void TestEquals_Null()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name = str;

        // Assert
        Assert.False(name.Equals(null));
    }

    [Fact]
    public void TestEquals_Correct()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;
        Name name2 = str;

        // Assert
        Assert.True(name1.Equals(name2));
    }

    [Fact]
    public void TestEquals_WrongType()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;
        int  name2 = 0;

        // Assert
        Assert.False(name1.Equals((object)name2));
    }

    [Fact]
    public void TestEquals_SameObjects()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;

        // Assert
        Assert.True(name1.Equals(name1));
    }

    [Fact]
    public void TestCtor_WrongLength_TooShort()
    {
        // Arrange
        const string str = "N";

        // Act
        static void Act() => _ = new Name(str);

        // Assert
        Assert.Throws<DomainException>(Act);
    }

    [Fact]
    public void TestCtor_WrongLength_TooLong()
    {
        // Arrange
        var str = new string('N', 51);

        // Act
        void Act() => _ = new Name(str);

        // Assert
        Assert.Throws<DomainException>(Act);
    }

    [Fact]
    public void TestCtor_Correct()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name = str;

        // Assert
        Assert.Equal(str, (string)name);
    }

    [Fact]
    public void TestToString_Correct()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name = str;

        // Assert
        Assert.Equal(str, name.ToString());
    }

    [Fact]
    public void TestGetHashCode_Correct()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name = str;
        Name name2 = str;

        // Assert
        Assert.Equal(name.GetHashCode(), name2.GetHashCode());
    }

    // Test CompareTo and operators

    [Fact]
    public void TestCompareTo_Correct()
    {
        // Arrange
        const string str1 = "Name1";
        const string str2 = "Name2";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.Equal(-1, name1.CompareTo(name2));
    }

    [Fact]
    public void TestCompareTo_Same()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;
        Name name2 = str;

        // Assert
        Assert.Equal(0, name1.CompareTo(name2));
    }

    [Fact]
    public void TestCompareTo_Null()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;

        // Assert
        Assert.Equal(1, name1.CompareTo(null));
    }

    [Fact]
    public void TestCompareTo_SameObjects()
    {
        // Arrange
        const string str = "Name";

        // Act
        Name name1 = str;

        // Assert
        Assert.Equal(0, name1.CompareTo(name1));
    }

    [Fact]
    public void TestCompareTo_OperatorLess()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aab";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 < name2);
    }

    [Fact]
    public void TestCompareTo_OperatorLessOrEqual()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aab";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 <= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreater()
    {
        // Arrange
        const string str1 = "aab";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 > name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreaterOrEqual()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 >= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorEqual()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 == name2);
    }

    [Fact]
    public void TestCompareTo_OperatorNotEqual()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aab";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 != name2);
    }

    // Strings different length

    [Fact]
    public void TestCompareTo_OperatorLess_DifferentLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 < name2);
    }

    [Fact]
    public void TestCompareTo_OperatorLessOrEqual_DifferentLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 <= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreater_DifferentLength()
    {
        // Arrange
        const string str1 = "aaba";
        const string str2 = "aab";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 > name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreaterOrEqual_DifferentLength()
    {
        // Arrange
        const string str1 = "aaaa";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 >= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorEqual_DifferentLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.False(name1 == name2);
    }

    [Fact]
    public void TestCompareTo_OperatorNotEqual_DifferentLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 != name2);
    }

    // Strings different case

    [Fact]
    public void TestCompareTo_OperatorLess_DifferentCase()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 > name2);
    }

    [Fact]
    public void TestCompareTo_OperatorLessOrEqual_DifferentCase()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 >= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreater_DifferentCase()
    {
        // Arrange
        const string str1 = "Aaa";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 < name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreaterOrEqual_DifferentCase()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 >= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorEqual_DifferentCase()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.False(name1 == name2);
    }

    [Fact]
    public void TestCompareTo_OperatorNotEqual_DifferentCase()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 != name2);
    }

    // Strings different case and length

    [Fact]
    public void TestCompareTo_OperatorLess_DifferentCaseAndLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 > name2);
    }

    [Fact]
    public void TestCompareTo_OperatorLessOrEqual_DifferentCaseAndLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 >= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreater_DifferentCaseAndLength()
    {
        // Arrange
        const string str1 = "Aaaa";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 < name2);
    }

    [Fact]
    public void TestCompareTo_OperatorGreaterOrEqual_DifferentCaseAndLength()
    {
        // Arrange
        const string str1 = "Aaaa";
        const string str2 = "aaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.True(name1 <= name2);
    }

    [Fact]
    public void TestCompareTo_OperatorEqual_DifferentCaseAndLength()
    {
        // Arrange
        const string str1 = "aaa";
        const string str2 = "Aaaa";

        // Act
        Name name1 = str1;
        Name name2 = str2;

        // Assert
        Assert.False(name1 == name2);
    }
}
