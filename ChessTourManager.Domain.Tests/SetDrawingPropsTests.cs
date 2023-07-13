using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Tests;

public sealed class SetDrawingPropsTests
{
    #region Create tournament

    [Fact]
    public void CreateSwiss_With_CorrectCoefficients_Should_SetAllProps()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Assert
        Assert.Equal(id,                                   tournament.Id);
        Assert.Equal(name,                                 tournament.Name);
        Assert.Equal(drawSystem,                           tournament.System);
        Assert.Equal(coefficients,                         tournament.Coefficients);
        Assert.Equal(maxTour,                              tournament.MaxTour);
        Assert.Equal(createdAt,                            tournament.CreatedAt);
        Assert.Equal(currentTour,                          tournament.CurrentTour);
        Assert.Equal(groups,                               tournament.Groups);
        Assert.Equal(TournamentBase.TournamentKind.Single, tournament.Kind);
    }

    [Fact]
    public void CreateSwiss_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.Berger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.CreateSingleTournament(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt, false, null));
        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Fact]
    public void CreateSwiss_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients = new()
                                                            {
                                                                TournamentBase.DrawCoefficient.Buchholz,
                                                                TournamentBase.DrawCoefficient.Berger,
                                                                TournamentBase.DrawCoefficient.SimpleBerger,
                                                            };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.CreateSingleTournament(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt, false, null));
        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger, SimpleBerger", exception!.Message);
    }

    [Fact]
    public void CreateSwiss_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.CreateSingleTournament(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt, false, null));
        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger, SimpleBerger", exception!.Message);
    }

    [Fact]
    public void CreateRoundRobin_With_CorrectCoefficients_Should_SetAllProps()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Assert
        Assert.Equal(id,                                   tournament.Id);
        Assert.Equal(name,                                 tournament.Name);
        Assert.Equal(drawSystem,                           tournament.System);
        Assert.Equal(coefficients,                         tournament.Coefficients);
        Assert.Equal(maxTour,                              tournament.MaxTour);
        Assert.Equal(createdAt,                            tournament.CreatedAt);
        Assert.Equal(currentTour,                          tournament.CurrentTour);
        Assert.Equal(groups,                               tournament.Groups);
        Assert.Equal(TournamentBase.TournamentKind.Single, tournament.Kind);
    }

    [Fact]
    public void CreateRoundRobin_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.Berger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.CreateSingleTournament(id, name, drawSystem,
                                                           coefficients, maxTour,
                                                           currentTour, groups, createdAt, false, null));
        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Fact]
    public void CreateRoundRobin_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients = new()
                                                            {
                                                                TournamentBase.DrawCoefficient.Buchholz,
                                                                TournamentBase.DrawCoefficient.Berger,
                                                                TournamentBase.DrawCoefficient.SimpleBerger,
                                                            };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.CreateSingleTournament(id, name, drawSystem,
                                                           coefficients,
                                                           maxTour,
                                                           currentTour, groups, createdAt, false, null));
        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Fact]
    public void CreateRoundRobin_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();

        // Act
        var exception = Assert.Throws<DomainException>(() => TournamentBase.CreateSingleTournament(id, name, drawSystem,
                                                           coefficients,
                                                           maxTour,
                                                           currentTour, groups, createdAt, false, null));
        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz, TotalBuchholz", exception!.Message);
    }

    #endregion CreateTournament

    #region Update coefficients for Round-Robin

    [Fact]
    public void UpdateCoefficients_RoundRobin_With_CorrectCoefficients_Should_SetCoefficients()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Berger });

        // Assert
        Assert.Equal(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Berger },
                        tournament.Coefficients);
    }

    [Fact]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient.Buchholz,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .Buchholz,
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .Buchholz,
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .TotalBuchholz,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz, TotalBuchholz", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_RoundRobin_With_IncorrectCoefficients_Should_Throw4()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .Buchholz,
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .TotalBuchholz,
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for RoundRobin draw system: Buchholz, TotalBuchholz", exception!.Message);
    }

    #endregion Update coefficients for Round-Robin

    #region Update coefficients for Swiss

    [Fact]
    public void UpdateCoefficients_Swiss_With_CorrectCoefficients_Should_SetCoefficients1()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                      { TournamentBase.DrawCoefficient.Buchholz });

        // Assert
        Assert.Equal(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Buchholz },
                        tournament.Coefficients);
    }

    [Fact]
    public void UpdateCoefficients_Swiss_With_CorrectCoefficients_Should_SetCoefficients2()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                      { TournamentBase.DrawCoefficient.TotalBuchholz });

        // Assert
        Assert.Equal(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.TotalBuchholz },
                        tournament.Coefficients);
    }

    [Fact]
    public void UpdateCoefficients_Swiss_With_CorrectCoefficients_Should_SetCoefficients3()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                      {
                                          TournamentBase.DrawCoefficient.Buchholz,
                                          TournamentBase.DrawCoefficient.TotalBuchholz,
                                      });

        // Assert
        Assert.Equal(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz },
                        tournament.Coefficients);
    }

    [Fact]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw1()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw2()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .Buchholz,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw3()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .TotalBuchholz,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_Swiss_With_IncorrectCoefficients_Should_Throw4()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .Buchholz,
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    [Fact]
    public void UpdateCoefficients_With_IncorrectCoefficients_Should_Throw5()
    {
        // Arrange
        Id<Guid>                        id         = Guid.NewGuid();
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 1;
        TourNumber  currentTour = 1;
        List<Group> groups      = new();
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Act
        var exception =
            Assert.Throws<DomainException>(() => tournament.UpdateCoefficients(new List<TournamentBase.DrawCoefficient>
                                                                               {
                                                                                   TournamentBase.DrawCoefficient
                                                                                      .TotalBuchholz,
                                                                                   TournamentBase.DrawCoefficient.Berger,
                                                                               }));

        // Assert
        Assert.Equal("Wrong coefficients for Swiss draw system: Berger", exception!.Message);
    }

    #endregion UpdateCoefficients for Swiss
}
