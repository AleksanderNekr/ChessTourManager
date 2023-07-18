namespace ChessTourManager.Domain.Interfaces.Tests;

public class DrawableTests
{
    private class TestPlayer : IPlayer<TestPlayer>
    {
        public Name Name { get; }

        public TestPlayer(Name name)
        {
            Name = name;
        }

        public decimal Points { get; }

        public uint Wins { get; }

        public uint Draws { get; }

        public uint Loses { get; }

        public bool IsActive { get; }

        public IEnumerable<TestPlayer> GetAllOpponents()
        {
            yield return new TestPlayer(new Name("John"));
        }

        public IEnumerable<TestPlayer> GetBlackOpponents()
        {
            yield return new TestPlayer(new Name("John"));
        }

        public IEnumerable<TestPlayer> GetWhiteOpponents()
        {
            yield return new TestPlayer(new Name("John"));
        }

        public void AddGameToHistory(GamePair<TestPlayer> pair, PlayerColor color)
        {
            throw new NotImplementedException();
        }

        public void SetActive()
        {
            throw new NotImplementedException();
        }

        public void SetInactive()
        {
            throw new NotImplementedException();
        }
    }

    private class TestDrawable : IDrawable<TestPlayer>
    {
        public DrawSystem System { get; set; }

        public IReadOnlyCollection<DrawCoefficient> Coefficients { get; set; }

        public TourNumber MaxTour { get; set; }

        public TourNumber CurrentTour { get; set; }

        public bool AllowInGroupGames { get; }

        public IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<TestPlayer>>> GamePairs =>
            new Dictionary<TourNumber, IReadOnlySet<GamePair<TestPlayer>>>();

        public DrawResult DrawSwiss()
        {
            // Implementation for Swiss draw
            return DrawResult.Success("Success");
        }

        public DrawResult DrawRoundRobin()
        {
            // Implementation for RoundRobin draw
            return DrawResult.Success("Success");
        }
    }

    [Fact]
    public void UpdateCoefficients_ValidCoefficients_CoefficientsUpdated()
    {
        // Arrange
        List<DrawCoefficient> coefficients = new()
                                             {
                                                 DrawCoefficient.Berger,
                                                 DrawCoefficient.SimpleBerger
                                             };
        IDrawable<TestPlayer> drawable = new TestDrawable();

        // Act
        drawable.UpdateCoefficients(coefficients);

        // Assert
        Assert.Equal(coefficients, drawable.Coefficients);
    }

    [Fact]
    public void UpdateCoefficients_InvalidCoefficients_ThrowsDomainException()
    {
        // Arrange
        List<DrawCoefficient> coefficients = new()
                                             {
                                                 DrawCoefficient.Berger,
                                                 DrawCoefficient.Buchholz // Invalid coefficient for the system
                                             };
        IDrawable<TestPlayer> drawable = new TestDrawable();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => drawable.UpdateCoefficients(coefficients));
        Assert.Contains("Wrong coefficients", exception.Message);
    }

    [Fact]
    public void SetTours_MaxTourGreaterThanCurrentTour_ToursSet()
    {
        // Arrange
        var                   maxTour     = new TourNumber(5);
        var                   currentTour = new TourNumber(3);
        IDrawable<TestPlayer> drawable    = new TestDrawable();

        // Act
        drawable.SetTours(maxTour, currentTour);

        // Assert
        Assert.Equal(maxTour,     drawable.MaxTour);
        Assert.Equal(currentTour, drawable.CurrentTour);
    }

    [Fact]
    public void SetTours_MaxTourLessThanCurrentTour_ThrowsDomainException()
    {
        // Arrange
        var                   maxTour     = new TourNumber(3);
        var                   currentTour = new TourNumber(5);
        IDrawable<TestPlayer> drawable    = new TestDrawable();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => drawable.SetTours(maxTour, currentTour));
        Assert.Contains("Max tour must be greater or equal to current tour", exception.Message);
    }

    [Fact]
    public void DrawNewTour_RoundRobin_DrawRoundRobinCalled()
    {
        // Arrange
        IDrawable<TestPlayer> drawable = new TestDrawable();
        drawable.SetDrawingProperties(DrawSystem.RoundRobin, new List<DrawCoefficient>());

        // Act
        DrawResult result = drawable.DrawNewTour();

        // Assert
        Assert.Equal(DrawResult.ResultType.Success, result.Result);
        // Add additional checks if required for the expected result of DrawRoundRobin()
    }

    [Fact]
    public void DrawNewTour_Swiss_DrawSwissCalled()
    {
        // Arrange
        IDrawable<TestPlayer> drawable = new TestDrawable();
        drawable.SetDrawingProperties(DrawSystem.Swiss, new List<DrawCoefficient>());

        // Act
        DrawResult result = drawable.DrawNewTour();

        // Assert
        Assert.Equal(DrawResult.ResultType.Success, result.Result);
    }

    [Fact]
    public void GetPossibleCoefficients_WrongDrawSystem()
    {
        // Arrange
        const DrawSystem system = (DrawSystem)1000;

        // Act & Assert
        Assert.Throws<DomainOutOfRangeException>(static () =>
                                                     IDrawable<TestPlayer>.GetPossibleCoefficients(system));
    }

    [Fact]
    public void DrawNewTour_WrongDrawSystem()
    {
        // Arrange
        const DrawSystem      system   = (DrawSystem)1000;
        IDrawable<TestPlayer> drawable = new TestDrawable();
        ((TestDrawable)drawable).System = system;

        // Act & Assert
        Assert.Throws<DomainOutOfRangeException>(() => drawable.DrawNewTour());
    }
}
