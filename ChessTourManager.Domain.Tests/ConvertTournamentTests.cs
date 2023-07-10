using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.ValueObjects;
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable MethodTooLong

namespace ChessTourManager.Domain.Tests;

[TestFixture]
public class ConvertTournamentTests
{
    #region Convert from Single to Single

    [Test]
    public void ConvertFromSingleToSingle1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt);
        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Single,              result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
    }

    [Test]
    public void ConvertFromSingleToSingle2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };

        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Single,              result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
    }

    #endregion Convert from Single to Single

    #region Convert from Single to Single-Team

    [Test]
    public void ConvertFromSingleToSingleTeam1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt);
        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,          result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
        Assert.AreEqual(new List<Team>(),         result.Teams);
    }

    [Test]
    public void ConvertFromSingleToSingleTeam2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };

        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();
        result.Teams.Add(new Team { Name = "Team1" });

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,          result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" } }, result.Teams);
    }

    #endregion Convert from Single to Single-Team

    #region Convert from Single to Team

    [Test]
    public void ConvertFromSingleToTeam1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt);
        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Team,                result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
        Assert.AreEqual(new List<Team>(),         result.Teams);
    }

    [Test]
    public void ConvertFromSingleToTeam2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();
        result.Teams.Add(new Team { Name = "Team1" });

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Team,                result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" } }, result.Teams);
    }

    #endregion Convert from Single to Team

    #region Convert from Single-Team to Single

    [Test]
    public void ConvertFromSingleTeamToSingle1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        List<Team>        teams        = new();

        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Single,              result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
    }

    [Test]
    public void ConvertFromSingleTeamToSingle2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>        teams        = new() { new Team { Name  = "Team1" } };

        var tournament =
            TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour,
                                                                      groups, createdAt, teams);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Single,              result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
    }

    #endregion Convert from Single-Team to Single

    #region Convert from Single-Team to Team

    [Test]
    public void ConvertFromSingleTeamToTeam1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        List<Team>        teams        = new();

        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Team,                result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
        Assert.AreEqual(new List<Team>(),         result.Teams);
    }

    [Test]
    public void ConvertFromSingleTeamToTeam2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>        teams        = new() { new Team { Name  = "Team1" } };

        var tournament =
            TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Team,                result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" } }, result.Teams);
    }

    #endregion Convert from Single-Team to Team

    #region Convert from Single-Team to Single-Team

    [Test]
    public void ConvertFromSingleTeamToSingleTeam1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        List<Team>        teams        = new();

        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,          result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
        Assert.AreEqual(new List<Team>(),         result.Teams);
    }

    [Test]
    public void ConvertFromSingleTeamToSingleTeam2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>        teams        = new() { new Team { Name  = "Team1" } };

        var tournament =
            TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,          result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" } }, result.Teams);
    }

    #endregion Convert from Single-Team to Single-Team

    #region Convert from Team to Single

    [Test]
    public void ConvertFromTeamToSingle1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        List<Team>        teams        = new();

        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Single,              result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
    }

    [Test]
    public void ConvertFromTeamToSingle2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>        teams        = new() { new Team { Name  = "Team1" } };

        var tournament =
            TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Single,              result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
    }

    #endregion Convert from Team to Single

    #region Convert from Team to Single-Team

    [Test]
    public void ConvertFromTeamToSingleTeam1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        List<Team>        teams        = new();

        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,          result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
        Assert.AreEqual(new List<Team>(),         result.Teams);
    }

    [Test]
    public void ConvertFromTeamToSingleTeam2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>        teams        = new() { new Team { Name  = "Team1" } };

        var tournament =
            TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,          result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" } }, result.Teams);
    }

    #endregion Convert from Team to Single-Team

    #region Convert from Team to Team

    [Test]
    public void ConvertFromTeamToTeam1()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.RoundRobin;
        List<Coefficient> coefficients = new();
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 1;
        TourNumber        currentTour  = 1;
        List<Group>       groups       = new();
        List<Team>        teams        = new();

        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid),       result.Id);
        Assert.AreEqual(new Name("Test"),         result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,    result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),  result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Team,                result.Kind);
        Assert.AreEqual(new TourNumber(1),        result.MaxTour);
        Assert.AreEqual(new TourNumber(1),        result.CurrentTour);
        Assert.AreEqual(new List<Group>(),        result.Groups);
        Assert.AreEqual(new List<Team>(),         result.Teams);
    }

    [Test]
    public void ConvertFromTeamToTeam2()
    {
        // Arrange
        var               guid         = Guid.NewGuid();
        Id<Guid>          id           = guid;
        Name              name         = "Test";
        const DrawSystem  drawSystem   = DrawSystem.Swiss;
        List<Coefficient> coefficients = new() { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        DateOnly          createdAt    = new(2021, 1, 1);
        TourNumber        maxTour      = 7;
        TourNumber        currentTour  = 2;
        List<Group>       groups       = new() { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        List<Team>        teams        = new() { new Team { Name  = "Team1" } };

        var tournament =
            TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        Assert.AreEqual(new Id<Guid>(guid), result.Id);
        Assert.AreEqual(new Name("Test"),   result.Name);
        Assert.AreEqual(DrawSystem.Swiss,   result.DrawSystem);
        Assert.AreEqual(new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                        result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.AreEqual(Kind.Team,                result.Kind);
        Assert.AreEqual(new TourNumber(7),        result.MaxTour);
        Assert.AreEqual(new TourNumber(2),        result.CurrentTour);
        Assert.AreEqual(new List<Group> { new() { Name = "Group1" }, new() { Name = "Group2" } },
                        result.Groups);
        Assert.AreEqual(new List<Team> { new() { Name = "Team1" } }, result.Teams);
    }

    #endregion Convert from Team to Team
}
