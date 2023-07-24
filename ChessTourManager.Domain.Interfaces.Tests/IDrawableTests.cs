namespace ChessTourManager.Domain.Interfaces.Tests;

public class DrawableTests
{
    private class TestParticipant : Participant<TestParticipant>
    {
        public TestParticipant(Id<Guid> id, Name name, bool isActive) : base(id, name, isActive)
        {
        }
    }

    private class TestDrawableTournament : DrawableTournament<TestParticipant>
    {
        public TestDrawableTournament()
            : this(new Guid(),
                   "Test",
                   DateOnly.FromDateTime(DateTime.Now),
                   false,
                   DrawSystem.RoundRobin,
                   new List<DrawCoefficient>(),
                   1)
        {
        }

        public TestDrawableTournament(Id<Guid>   id,         Name name, DateOnly createdAt, bool allowMixGroupGames,
                                      DrawSystem drawSystem, IReadOnlyCollection<DrawCoefficient> coefficients,
                                      TourNumber maxTour,    TourNumber currentTour = default,
                                      IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<TestParticipant>>>?
                                          gamePairs = default) : base(id,
                                                                      name,
                                                                      createdAt,
                                                                      allowMixGroupGames,
                                                                      drawSystem,
                                                                      coefficients,
                                                                      maxTour,
                                                                      currentTour,
                                                                      gamePairs)
        {
        }

        public override SingleTournament ConvertToSingleTournament()
        {
            throw new NotImplementedException();
        }

        public override TeamTournament ConvertToTeamTournament()
        {
            throw new NotImplementedException();
        }

        public override SingleTeamTournament ConvertToSingleTeamTournament()
        {
            throw new NotImplementedException();
        }

        private protected override DrawResult DrawSwiss()
        {
            return DrawResult.Success("OK");
        }

        private protected override DrawResult DrawRoundRobin()
        {
            return DrawResult.Success("OK");
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
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();

        // Act
        drawableTournament.UpdateCoefficients(coefficients);

        // Assert
        Assert.Equal(coefficients, drawableTournament.Coefficients);
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
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => drawableTournament.UpdateCoefficients(coefficients));
        Assert.Contains("Wrong coefficients", exception.Message);
    }

    [Fact]
    public void SetTours_MaxTourGreaterThanCurrentTour_ToursSet()
    {
        // Arrange
        var                                 maxTour            = new TourNumber(5);
        var                                 currentTour        = new TourNumber(3);
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();

        // Act
        drawableTournament.SetTours(maxTour, currentTour);

        // Assert
        Assert.Equal(maxTour,     drawableTournament.MaxTour);
        Assert.Equal(currentTour, drawableTournament.CurrentTour);
    }

    [Fact]
    public void SetTours_MaxTourLessThanCurrentTour_ThrowsDomainException()
    {
        // Arrange
        var                                 maxTour            = new TourNumber(3);
        var                                 currentTour        = new TourNumber(5);
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => drawableTournament.SetTours(maxTour, currentTour));
        Assert.Contains("Max tour must be greater or equal to current tour", exception.Message);
    }

    [Fact]
    public void DrawNewTour_RoundRobin_DrawRoundRobinCalled()
    {
        // Arrange
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();
        drawableTournament.SetDrawingProperties(DrawSystem.RoundRobin, new List<DrawCoefficient>());

        // Act
        DrawResult result = drawableTournament.DrawNewTour();

        // Assert
        Assert.Equal(DrawResult.ResultType.Success, result.Result);
        // Add additional checks if required for the expected result of DrawRoundRobin()
    }

    [Fact]
    public void DrawNewTour_Swiss_DrawSwissCalled()
    {
        // Arrange
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();
        drawableTournament.SetDrawingProperties(DrawSystem.Swiss, new List<DrawCoefficient>());

        // Act
        DrawResult result = drawableTournament.DrawNewTour();

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
                                                     DrawableTournament<TestParticipant>.GetPossibleCoefficients(system));
    }

    [Fact]
    public void SetWrongDrawSystem()
    {
        // Arrange
        const DrawSystem                    system             = (DrawSystem)1000;
        DrawableTournament<TestParticipant> drawableTournament = new TestDrawableTournament();

        // Act & Assert
        Assert.Throws<DomainOutOfRangeException>(() => drawableTournament.SetDrawingProperties(system,
                                                     new List<DrawCoefficient>()));
    }
}
