namespace ChessTourManager.Domain.ValueObjects.Tests;

public class TourNumberTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(21)]
    public void TourNumber_InvalidValues_ThrowsDomainException(int value)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() => new TourNumber(value));
    }

    [Fact]
    public void TourNumber_ValidValue_CreatesTourNumber()
    {
        // Arrange
        int validValue = 10;

        // Act
        var tourNumber = new TourNumber(validValue);

        // Assert
        Assert.Equal(validValue, (int)tourNumber);
    }

    [Fact]
    public void NextTourNumber_ValidValue_ReturnsNextNumber()
    {
        // Arrange
        var tourNumber         = new TourNumber(5);
        int expectedNextNumber = 6;

        // Act
        var nextTourNumber = tourNumber.NextTourNumber();

        // Assert
        Assert.Equal(expectedNextNumber, (int)nextTourNumber);
    }

    [Fact]
    public void NextTourNumber_MaxValue_ThrowsDomainException()
    {
        // Arrange
        var tourNumber = new TourNumber(TourNumber.MaxValue);

        // Act & Assert
        Assert.Throws<DomainException>(() => tourNumber.NextTourNumber());
    }

    [Fact]
    public void PreviousTourNumber_ValidValue_ReturnsPreviousNumber()
    {
        // Arrange
        var tourNumber             = new TourNumber(10);
        int expectedPreviousNumber = 9;

        // Act
        var previousTourNumber = tourNumber.PreviousTourNumber();

        // Assert
        Assert.Equal(expectedPreviousNumber, (int)previousTourNumber);
    }

    [Fact]
    public void PreviousTourNumber_MinValue_ThrowsDomainException()
    {
        // Arrange
        var tourNumber = new TourNumber(TourNumber.MinValue);

        // Act & Assert
        Assert.Throws<DomainException>(() => tourNumber.PreviousTourNumber());
    }

    [Fact]
    public void ImplicitConversion_FromInt_CreatesTourNumber()
    {
        // Arrange
        int value = 15;

        // Act
        TourNumber tourNumber = value;

        // Assert
        Assert.Equal(value, (int)tourNumber);
    }

    [Fact]
    public void ImplicitConversion_ToInt_CreatesInt()
    {
        // Arrange
        var tourNumber = new TourNumber(7);

        // Act
        int value = tourNumber;

        // Assert
        Assert.Equal(7, value);
    }

    [Fact]
    public void ToString_ReturnsStringValue()
    {
        // Arrange
        var tourNumber = new TourNumber(12);

        // Act
        string stringValue = tourNumber.ToString();

        // Assert
        Assert.Equal("12", stringValue);
    }

    [Fact]
    public void GetHashCode_ReturnsHashValue()
    {
        // Arrange
        int value      = 9;
        var tourNumber = new TourNumber(value);

        // Act
        int hashCode = tourNumber.GetHashCode();

        // Assert
        Assert.Equal(value, hashCode);
    }

    [Theory]
    [InlineData(3, 3, true)]
    [InlineData(5, 2, false)]
    public void Equals_CompareToDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1.Equals(tourNumber2);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(3, 3, true)]
    [InlineData(5, 2, false)]
    public void Equals_CompareToDifferentValues_EqualsMethod(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1.Equals((object)tourNumber2);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(3, 5, -1)]
    [InlineData(7, 2, 1)]
    [InlineData(4, 4, 0)]
    public void CompareTo_CompareDifferentValues(int value1, int value2, int expectedComparisonResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        int comparisonResult = tourNumber1.CompareTo(tourNumber2);

        // Assert
        Assert.Equal(expectedComparisonResult, comparisonResult);
    }

    [Theory]
    [InlineData(8, 8, true)]
    [InlineData(2, 9, false)]
    public void OperatorEquals_CompareDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1 == tourNumber2;

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(6, 10, true)]
    [InlineData(1, 1,  false)]
    public void OperatorNotEquals_CompareDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1 != tourNumber2;

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(6,  7,  true)]
    [InlineData(15, 10, false)]
    public void OperatorLessThan_CompareDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1 < tourNumber2;

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(6, 3,  true)]
    [InlineData(7, 12, false)]
    public void OperatorGreaterThan_CompareDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1 > tourNumber2;

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(3,  5,  true)]
    [InlineData(17, 10, false)]
    [InlineData(6,  6,  true)]
    public void OperatorLessThanOrEqual_CompareDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1 <= tourNumber2;

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(7, 3, true)]
    [InlineData(4, 9, false)]
    [InlineData(5, 5, true)]
    public void OperatorGreaterThanOrEqual_CompareDifferentValues(int value1, int value2, bool expectedResult)
    {
        // Arrange
        var tourNumber1 = new TourNumber(value1);
        var tourNumber2 = new TourNumber(value2);

        // Act
        bool result = tourNumber1 >= tourNumber2;

        // Assert
        Assert.Equal(expectedResult, result);
    }
}