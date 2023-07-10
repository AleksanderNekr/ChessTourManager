using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Tests;

public sealed class SetDrawingPropsTests
{
    #region Create tournament

    [Test]
    public void CreateSwiss_With_CorrectCoefficients_Should_SetAllProps()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Assert
        Assert.AreEqual(id,           tournament.Id);
        Assert.AreEqual(name,         tournament.Name);
        Assert.AreEqual(drawSystem,   tournament.DrawSystem);
        Assert.AreEqual(coefficients, tournament.Coefficients);
        Assert.AreEqual(maxTour,      tournament.MaxTour);
        Assert.AreEqual(createdAt,    tournament.CreatedAt);
        Assert.AreEqual(currentTour,  tournament.CurrentTour);
        Assert.AreEqual(groups,       tournament.Groups);
        Assert.AreEqual(Kind.Single,  tournament.Kind);
    }

    [Test]
    public void CreateSwiss_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.Berger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.Create(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt));
        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Test]
    public void CreateSwiss_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        // Assert
        Assert.Throws<DomainException>(() => TournamentBase.Create(id, name, drawSystem, coefficients, maxTour,
                                                                   currentTour, groups, createdAt),
                                       "Wrong coefficients for Swiss draw system: Berger, SimpleBerger");
    }

    [Test]
    public void CreateSwiss_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.Create(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt));
        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger, SimpleBerger", exception!.Message);
    }

    [Test]
    public void CreateRoundRobin_With_CorrectCoefficients_Should_SetAllProps()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Assert
        Assert.AreEqual(id,           tournament.Id);
        Assert.AreEqual(name,         tournament.Name);
        Assert.AreEqual(drawSystem,   tournament.DrawSystem);
        Assert.AreEqual(coefficients, tournament.Coefficients);
        Assert.AreEqual(maxTour,      tournament.MaxTour);
        Assert.AreEqual(createdAt,    tournament.CreatedAt);
        Assert.AreEqual(currentTour,  tournament.CurrentTour);
        Assert.AreEqual(groups,       tournament.Groups);
        Assert.AreEqual(Kind.Single,  tournament.Kind);
    }

    [Test]
    public void CreateRoundRobin_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.Berger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.Create(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt));
        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Test]
    public void CreateRoundRobin_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.Create(id, name, drawSystem, coefficients,
                                                           maxTour,
                                                           currentTour, groups, createdAt));
        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Test]
    public void CreateRoundRobin_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.Create(id, name, drawSystem, coefficients,
                                                           maxTour,
                                                           currentTour, groups, createdAt));
        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz, TotalBuchholz", exception!.Message);
    }

    #endregion CreateTournament

    #region Update coefficients for Round-Robin

    [Test]
    public void UpdateCoefficients_RoundRobin_With_CorrectCoefficients_Should_SetCoefficients()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        tournament.UpdateCoefficients(new List<Coefficient> { Coefficient.Berger });

        // Assert
        Assert.AreEqual(new List<Coefficient> { Coefficient.Berger }, tournament.Coefficients);
    }

    [Test]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               { Coefficient.Buchholz }));

        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.Buchholz,
                                                                                   Coefficient.Berger
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.Buchholz,
                                                                                   Coefficient.TotalBuchholz
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz, TotalBuchholz", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw4()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new() { Coefficient.Berger, Coefficient.SimpleBerger };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.Buchholz,
                                                                                   Coefficient.TotalBuchholz,
                                                                                   Coefficient.Berger
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for RoundRobin draw system: Buchholz, TotalBuchholz", exception!.Message);
    }

    #endregion Update coefficients for Round-Robin

    #region Update coefficients for Swiss

    [Test]
    public void UpdateCoefficients_Swiss_With_CorrectCoefficients_Should_SetCoefficients1()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        tournament.UpdateCoefficients(new List<Coefficient> { Coefficient.Buchholz });

        // Assert
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz }, tournament.Coefficients);
    }

    [Test]
    public void UpdateCoefficients_Swiss_With_CorrectCoefficients_Should_SetCoefficients2()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        tournament.UpdateCoefficients(new List<Coefficient> { Coefficient.TotalBuchholz });

        // Assert
        Assert.AreEqual(new List<Coefficient> { Coefficient.TotalBuchholz }, tournament.Coefficients);
    }

    [Test]
    public void UpdateCoefficients_Swiss_With_CorrectCoefficients_Should_SetCoefficients3()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        tournament.UpdateCoefficients(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz });

        // Assert
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        tournament.Coefficients);
    }

    [Test]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               { Coefficient.Berger }));

        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.Berger,
                                                                                   Coefficient.Buchholz
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.Berger,
                                                                                   Coefficient.TotalBuchholz
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw4()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.Buchholz,
                                                                                   Coefficient.Berger
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Test]
    public void UpdateCoefficients_With_IncorrectCoefficients_Should_Throw5()
    {
        // Arrange
        Id<Guid>          id           = Guid.NewGuid();
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        SingleTournament tournament =
            TournamentBase.Create(id, name, drawSystem, coefficients, maxTour, currentTour, groups, createdAt);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<Coefficient>
                                                                               {
                                                                                   Coefficient.TotalBuchholz,
                                                                                   Coefficient.Berger
                                                                               }));

        // Assert
        Assert.AreEqual("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    #endregion UpdateCoefficients for Swiss
}
