using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Tests;

public sealed class CreateTournamentsTests
{
    #region Create Single tournament

    [Test]
    public void CreateSingleTournament_Swiss_Should_SetAllProps1()
    {
        // Arrange
        var                                  guid         = Guid.NewGuid();
        Id<Guid>                             id           = guid;
        Name                                 name         = "Test";
        const TournamentBase.DrawSystem      drawSystem   = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients = new();
        DateOnly                             createdAt    = new(2021, 1, 1);
        TourNumber                           maxTour      = 1;
        TourNumber                           currentTour  = 1;
        List<Group>                          groups       = new();

        // Act
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                         tournament.Id);
        Assert.AreEqual(new Name("Test"),                           tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.Swiss,            tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient>(), tournament.Coefficients);
        Assert.AreEqual(new TourNumber(1),                          tournament.MaxTour);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                   tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(1),                          tournament.CurrentTour);
        Assert.AreEqual(new List<Group>(),                          tournament.Groups);
        Assert.AreEqual(TournamentBase.TournamentKind.Single,       tournament.Kind);
    }

    [Test]
    public void CreateSingleTournament_Swiss_Should_SetAllProps2()
    {
        // Arrange
        var                             guid       = Guid.NewGuid();
        Id<Guid>                        id         = guid;
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 7;
        TourNumber  currentTour = 2;
        List<Group> groups      = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };

        // Act
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),              tournament.Id);
        Assert.AreEqual(new Name("Test"),                tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.Swiss, tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz },
                        tournament.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(7),        tournament.MaxTour);
        Assert.AreEqual(new TourNumber(2),        tournament.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        tournament.Groups);
        Assert.AreEqual(TournamentBase.TournamentKind.Single, tournament.Kind);
    }

    [Test]
    public void CreateSingleTournament_RoundRobin_Should_SetAllProps1()
    {
        // Arrange
        var                                  guid         = Guid.NewGuid();
        Id<Guid>                             id           = guid;
        Name                                 name         = "Test";
        const TournamentBase.DrawSystem      drawSystem   = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients = new();
        DateOnly                             createdAt    = new(2021, 1, 1);
        TourNumber                           maxTour      = 1;
        TourNumber                           currentTour  = 1;
        List<Group>                          groups       = new();

        // Act
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                         tournament.Id);
        Assert.AreEqual(new Name("Test"),                           tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.RoundRobin,       tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient>(), tournament.Coefficients);
        Assert.AreEqual(new TourNumber(1),                          tournament.MaxTour);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                   tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(1),                          tournament.CurrentTour);
        Assert.AreEqual(new List<Group>(),                          tournament.Groups);
        Assert.AreEqual(TournamentBase.TournamentKind.Single,       tournament.Kind);
    }

    [Test]
    public void CreateSingleTournament_RoundRobin_Should_SetAllProps2()
    {
        // Arrange
        var                             guid       = Guid.NewGuid();
        Id<Guid>                        id         = guid;
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 7;
        TourNumber  currentTour = 2;
        List<Group> groups      = new() { new Group { Name = "Group1" } };

        // Act
        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                   tournament.Id);
        Assert.AreEqual(new Name("Test"),                     tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.RoundRobin, tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger },
                        tournament.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(7),        tournament.MaxTour);
        Assert.AreEqual(new TourNumber(2),        tournament.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" } },
                        tournament.Groups);
        Assert.AreEqual(TournamentBase.TournamentKind.Single, tournament.Kind);
    }

    #endregion Create Single Tournament

    #region Create Team Tournament

    [Test]
    public void CreateTeamTournament_Swiss_Should_SetAllProps1()
    {
        // Arrange
        var                                  guid         = Guid.NewGuid();
        Id<Guid>                             id           = guid;
        Name                                 name         = "Test";
        const TournamentBase.DrawSystem      drawSystem   = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients = new();
        DateOnly                             createdAt    = new(2021, 1, 1);
        TourNumber                           maxTour      = 1;
        TourNumber                           currentTour  = 1;
        List<Group>                          groups       = new();
        List<Team>                           teams        = new();

        // Act
        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                             currentTour,
                                                                             groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                         tournament.Id);
        Assert.AreEqual(new Name("Test"),                           tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.Swiss,            tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient>(), tournament.Coefficients);
        Assert.AreEqual(new TourNumber(1),                          tournament.MaxTour);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                   tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(1),                          tournament.CurrentTour);
        Assert.AreEqual(new List<Group>(),                          tournament.Groups);
        Assert.AreEqual(new List<Team>(),                           tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.Team,         tournament.Kind);
    }

    [Test]
    public void CreateTeamTournament_Swiss_Should_SetAllProps2()
    {
        // Arrange
        var                             guid       = Guid.NewGuid();
        Id<Guid>                        id         = guid;
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 7;
        TourNumber  currentTour = 2;
        List<Group> groups      = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>  teams       = new() { new Team { Name  = "Team1" }, new Team { Name   = "Team2" } };

        // Act
        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                             currentTour,
                                                                             groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),              tournament.Id);
        Assert.AreEqual(new Name("Test"),                tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.Swiss, tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz },
                        tournament.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(7),        tournament.MaxTour);
        Assert.AreEqual(new TourNumber(2),        tournament.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        tournament.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" }, new() { Name = "Team2" } },
                        tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.Team, tournament.Kind);
    }

    [Test]
    public void CreateTeamTournament_RoundRobin_Should_SetAllProps1()
    {
        // Arrange
        var                                  guid         = Guid.NewGuid();
        Id<Guid>                             id           = guid;
        Name                                 name         = "Test";
        const TournamentBase.DrawSystem      drawSystem   = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients = new();
        DateOnly                             createdAt    = new(2021, 1, 1);
        TourNumber                           maxTour      = 1;
        TourNumber                           currentTour  = 1;
        List<Group>                          groups       = new();
        List<Team>                           teams        = new();

        // Act
        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                             currentTour,
                                                                             groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                         tournament.Id);
        Assert.AreEqual(new Name("Test"),                           tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.RoundRobin,       tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient>(), tournament.Coefficients);
        Assert.AreEqual(new TourNumber(1),                          tournament.MaxTour);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                   tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(1),                          tournament.CurrentTour);
        Assert.AreEqual(new List<Group>(),                          tournament.Groups);
        Assert.AreEqual(new List<Team>(),                           tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.Team,         tournament.Kind);
    }

    [Test]
    public void CreateTeamTournament_RoundRobin_Should_SetAllProps2()
    {
        // Arrange
        var                             guid       = Guid.NewGuid();
        Id<Guid>                        id         = guid;
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 7;
        TourNumber  currentTour = 2;
        List<Group> groups      = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>  teams       = new() { new Team { Name  = "Team1" }, new Team { Name   = "Team2" } };

        // Act
        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                             currentTour,
                                                                             groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                   tournament.Id);
        Assert.AreEqual(new Name("Test"),                     tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.RoundRobin, tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger },
                        tournament.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(7),        tournament.MaxTour);
        Assert.AreEqual(new TourNumber(2),        tournament.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        tournament.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" }, new() { Name = "Team2" } },
                        tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.Team, tournament.Kind);
    }

    #endregion Create Team Tournament

    #region Create Single-Team Tournament

    [Test]
    public void CreateSingleTeamTournament_Swiss_Should_SetAllProps1()
    {
        // Arrange
        var                                  guid         = Guid.NewGuid();
        Id<Guid>                             id           = guid;
        Name                                 name         = "Test";
        const TournamentBase.DrawSystem      drawSystem   = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients = new();
        DateOnly                             createdAt    = new(2021, 1, 1);
        TourNumber                           maxTour      = 1;
        TourNumber                           currentTour  = 1;
        List<Group>                          groups       = new();
        List<Team>                           teams        = new();

        // Act
        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour,
            currentTour, groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                         tournament.Id);
        Assert.AreEqual(new Name("Test"),                           tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.Swiss,            tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient>(), tournament.Coefficients);
        Assert.AreEqual(new TourNumber(1),                          tournament.MaxTour);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                   tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(1),                          tournament.CurrentTour);
        Assert.AreEqual(new List<Group>(),                          tournament.Groups);
        Assert.AreEqual(new List<Team>(),                           tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.SingleTeam,   tournament.Kind);
    }

    [Test]
    public void CreateSingleTeamTournament_Swiss_Should_SetAllProps2()
    {
        // Arrange
        var                             guid       = Guid.NewGuid();
        Id<Guid>                        id         = guid;
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.Swiss;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 7;
        TourNumber  currentTour = 2;
        List<Group> groups      = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>  teams       = new() { new Team { Name  = "Team1" }, new Team { Name   = "Team2" } };

        // Act
        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour,
            currentTour, groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),              tournament.Id);
        Assert.AreEqual(new Name("Test"),                tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.Swiss, tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz },
                        tournament.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(7),        tournament.MaxTour);
        Assert.AreEqual(new TourNumber(2),        tournament.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        tournament.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" }, new() { Name = "Team2" } },
                        tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.SingleTeam, tournament.Kind);
    }

    [Test]
    public void CreateSingleTeamTournament_RoundRobin_Should_SetAllProps1()
    {
        // Arrange
        var                                  guid         = Guid.NewGuid();
        Id<Guid>                             id           = guid;
        Name                                 name         = "Test";
        const TournamentBase.DrawSystem      drawSystem   = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients = new();
        DateOnly                             createdAt    = new(2021, 1, 1);
        TourNumber                           maxTour      = 1;
        TourNumber                           currentTour  = 1;
        List<Group>                          groups       = new();
        List<Team>                           teams        = new();

        // Act
        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour,
            currentTour, groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                         tournament.Id);
        Assert.AreEqual(new Name("Test"),                           tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.RoundRobin,       tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient>(), tournament.Coefficients);
        Assert.AreEqual(new TourNumber(1),                          tournament.MaxTour);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                   tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(1),                          tournament.CurrentTour);
        Assert.AreEqual(new List<Group>(),                          tournament.Groups);
        Assert.AreEqual(new List<Team>(),                           tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.SingleTeam,   tournament.Kind);
    }

    [Test]
    public void CreateSingleTeamTournament_RoundRobin_Should_SetAllProps2()
    {
        // Arrange
        var                             guid       = Guid.NewGuid();
        Id<Guid>                        id         = guid;
        Name                            name       = "Test";
        const TournamentBase.DrawSystem drawSystem = TournamentBase.DrawSystem.RoundRobin;
        List<TournamentBase.DrawCoefficient> coefficients =
            new() { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger };
        DateOnly    createdAt   = new(2021, 1, 1);
        TourNumber  maxTour     = 7;
        TourNumber  currentTour = 2;
        List<Group> groups      = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>  teams       = new() { new Team { Name  = "Team1" }, new Team { Name   = "Team2" } };

        // Act
        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour,
            currentTour, groups, createdAt, teams, false, null);

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),                   tournament.Id);
        Assert.AreEqual(new Name("Test"),                     tournament.Name);
        Assert.AreEqual(TournamentBase.DrawSystem.RoundRobin, tournament.System);
        Assert.AreEqual(new List<TournamentBase.DrawCoefficient> { TournamentBase.DrawCoefficient.Berger, TournamentBase.DrawCoefficient.SimpleBerger },
                        tournament.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.AreEqual(new TourNumber(7),        tournament.MaxTour);
        Assert.AreEqual(new TourNumber(2),        tournament.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        tournament.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" }, new() { Name = "Team2" } },
                        tournament.Teams);
        Assert.AreEqual(TournamentBase.TournamentKind.SingleTeam, tournament.Kind);
    }

    #endregion Create Single-Team Tournament
}
