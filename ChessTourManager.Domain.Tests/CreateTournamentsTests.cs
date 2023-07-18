using System.Collections.Immutable;
using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable MethodTooLong

namespace ChessTourManager.Domain.Tests;

public sealed class CreateTournamentsTests
{
    private readonly ImmutableArray<Guid> _guids;

    private readonly ImmutableArray<string> _groupNames;

    private readonly ImmutableArray<string> _teamNames;

    private readonly ImmutableArray<Player> _players;

    public CreateTournamentsTests()
    {
        this._teamNames  = ImmutableArray.Create("Team1",        "Team2",        "Team3",        "Team4");
        this._groupNames = ImmutableArray.Create("Group1",       "Group2",       "Group3",       "Group4");
        this._guids      = ImmutableArray.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        ImmutableArray<string> playerNames = ImmutableArray.Create("Player1", "Player2", "Player3", "Player4");

        this._players = ImmutableArray.Create(new Player(this._guids[0], playerNames[0]),
                                              new Player(this._guids[1], playerNames[1]),
                                              new Player(this._guids[2], playerNames[2]),
                                              new Player(this._guids[3], playerNames[3]));
    }

    #region Create Single tournament

    [Fact]
    public void SingleTournament_Swiss_Should_SetAllProps1()
    {
        // Arrange
        var                   guid         = Guid.NewGuid();
        Id<Guid>              id           = guid;
        Name                  name         = "Test";
        const DrawSystem      drawSystem   = DrawSystem.Swiss;
        List<DrawCoefficient> coefficients = new();
        DateOnly              createdAt    = new(2021, 1, 1);
        TourNumber            maxTour      = 1;
        TourNumber            currentTour  = 1;
        List<Group>           groups       = new();

        // Act
        SingleTournament tournament = new SingleTournament(id,
                                                           name,
                                                           drawSystem,
                                                           coefficients,
                                                           maxTour,
                                                           createdAt,
                                                           false)
                                      {
                                          GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                                          Groups    = new HashSet<Group>(groups),
                                      };

        // Assert
        Assert.Equal(new Id<Guid>(guid),                   tournament.Id);
        Assert.Equal(new Name("Test"),                     tournament.Name);
        Assert.Equal(DrawSystem.Swiss,                     tournament.System);
        Assert.Equal(new List<DrawCoefficient>(),          tournament.Coefficients);
        Assert.Equal(new TourNumber(1),                    tournament.MaxTour);
        Assert.Equal(new DateOnly(2021, 1, 1),             tournament.CreatedAt);
        Assert.Equal(new TourNumber(1),                    tournament.CurrentTour);
        Assert.Equal(new List<Group>(),                    tournament.Groups);
        Assert.Equal(TournamentKind.Single, tournament.Kind);
    }

    [Fact]
    public void SingleTournament_Swiss_Should_SetAllProps2()
    {
        // Arrange
        var              guid       = Guid.NewGuid();
        Id<Guid>         id         = guid;
        Name             name       = "Test";
        const DrawSystem drawSystem = DrawSystem.Swiss;
        List<DrawCoefficient> coefficients =
            new() { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz };
        DateOnly   createdAt   = new(2021, 1, 1);
        TourNumber maxTour     = 7;
        TourNumber currentTour = 2;
        List<Group> groups = new()
                             {
                                 new(this._guids[0], this._groupNames[0], this._players[..2]),
                                 new(this._guids[1], this._groupNames[1], this._players[2..]),
                             };

        // Act
        SingleTournament tournament = new SingleTournament(id,
                                                           name,
                                                           drawSystem,
                                                           coefficients,
                                                           maxTour,
                                                           createdAt,
                                                           false)
                                      {
                                          GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                                          Groups    = new HashSet<Group>(groups),
                                      };

        // Assert
        Assert.Equal(new Id<Guid>(guid), tournament.Id);
        Assert.Equal(new Name("Test"),   tournament.Name);
        Assert.Equal(DrawSystem.Swiss,   tournament.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz },
                     tournament.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.Equal(new TourNumber(7),        tournament.MaxTour);
        Assert.Equal(new TourNumber(1),        tournament.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     tournament.Groups);
        Assert.Equal(TournamentKind.Single, tournament.Kind);
    }

    [Fact]
    public void SingleTournament_RoundRobin_Should_SetAllProps1()
    {
        // Arrange
        var                   guid         = Guid.NewGuid();
        Id<Guid>              id           = guid;
        Name                  name         = "Test";
        const DrawSystem      drawSystem   = DrawSystem.RoundRobin;
        List<DrawCoefficient> coefficients = new();
        DateOnly              createdAt    = new(2021, 1, 1);
        TourNumber            maxTour      = 1;
        TourNumber            currentTour  = 1;
        List<Group>           groups       = new();

        // Act
        SingleTournament tournament = new SingleTournament(id,
                                                           name,
                                                           drawSystem,
                                                           coefficients,
                                                           maxTour,
                                                           createdAt,
                                                           false)
                                      {
                                          GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                                          Groups    = new HashSet<Group>(groups),
                                      };

        // Assert
        Assert.Equal(new Id<Guid>(guid),                   tournament.Id);
        Assert.Equal(new Name("Test"),                     tournament.Name);
        Assert.Equal(DrawSystem.RoundRobin,                tournament.System);
        Assert.Equal(new List<DrawCoefficient>(),          tournament.Coefficients);
        Assert.Equal(new TourNumber(1),                    tournament.MaxTour);
        Assert.Equal(new DateOnly(2021, 1, 1),             tournament.CreatedAt);
        Assert.Equal(new TourNumber(1),                    tournament.CurrentTour);
        Assert.Equal(new List<Group>(),                    tournament.Groups);
        Assert.Equal(TournamentKind.Single, tournament.Kind);
    }

    [Fact]
    public void SingleTournament_RoundRobin_Should_SetAllProps2()
    {
        // Arrange
        var              guid       = Guid.NewGuid();
        Id<Guid>         id         = guid;
        Name             name       = "Test";
        const DrawSystem drawSystem = DrawSystem.RoundRobin;
        List<DrawCoefficient> coefficients =
            new() { DrawCoefficient.Berger, DrawCoefficient.SimpleBerger };
        DateOnly   createdAt   = new(2021, 1, 1);
        TourNumber maxTour     = 7;
        TourNumber currentTour = 2;
        List<Group> groups = new()
                             {
                                 new(this._guids[0], this._groupNames[0], this._players[..2]),
                                 new(this._guids[1], this._groupNames[1], this._players[2..]),
                             };

        // Act
        SingleTournament tournament = new SingleTournament(id,
                                                           name,
                                                           drawSystem,
                                                           coefficients,
                                                           maxTour,
                                                           createdAt,
                                                           false)
                                      {
                                          GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                                          Groups    = new HashSet<Group>(groups),
                                      };

        // Assert
        Assert.Equal(new Id<Guid>(guid),    tournament.Id);
        Assert.Equal(new Name("Test"),      tournament.Name);
        Assert.Equal(DrawSystem.RoundRobin, tournament.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Berger, DrawCoefficient.SimpleBerger },
                     tournament.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.Equal(new TourNumber(7),        tournament.MaxTour);
        Assert.Equal(new TourNumber(1),        tournament.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     tournament.Groups);
        Assert.Equal(TournamentKind.Single, tournament.Kind);
    }

    #endregion Create Single Tournament

    #region Create Team Tournament

    [Fact]
    public void CreateTeamTournament_Swiss_Should_SetAllProps1()
    {
        // Arrange
        var                   guid         = Guid.NewGuid();
        Id<Guid>              id           = guid;
        Name                  name         = "Test";
        const DrawSystem      drawSystem   = DrawSystem.Swiss;
        List<DrawCoefficient> coefficients = new();
        DateOnly              createdAt    = new(2021, 1, 1);
        TourNumber            maxTour      = 1;
        TourNumber            currentTour  = 1;
        List<Group>           groups       = new();
        List<Team>            teams        = new();

        // Act
        var tournament = new TeamTournament(id,
                                            name,
                                            drawSystem,
                                            coefficients,
                                            maxTour,
                                            createdAt,
                                            false)
                         {
                             Teams = new HashSet<Team>(teams),
                             GamePairs =
                                 new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(),
                             Groups = new HashSet<Group>(groups),
                         };
        ;

        // Assert
        Assert.Equal(new Id<Guid>(guid),                 tournament.Id);
        Assert.Equal(new Name("Test"),                   tournament.Name);
        Assert.Equal(DrawSystem.Swiss,                   tournament.System);
        Assert.Equal(new List<DrawCoefficient>(),        tournament.Coefficients);
        Assert.Equal(new TourNumber(1),                  tournament.MaxTour);
        Assert.Equal(new DateOnly(2021, 1, 1),           tournament.CreatedAt);
        Assert.Equal(new TourNumber(1),                  tournament.CurrentTour);
        Assert.Equal(new List<Group>(),                  tournament.Groups);
        Assert.Equal(new List<Team>(),                   tournament.Teams);
        Assert.Equal(TournamentKind.Team, tournament.Kind);
    }

    [Fact]
    public void CreateTeamTournament_Swiss_Should_SetAllProps2()
    {
        // Arrange
        var              guid       = Guid.NewGuid();
        Id<Guid>         id         = guid;
        Name             name       = "Test";
        const DrawSystem drawSystem = DrawSystem.Swiss;
        List<DrawCoefficient> coefficients =
            new() { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz };
        DateOnly   createdAt   = new(2021, 1, 1);
        TourNumber maxTour     = 7;
        TourNumber currentTour = 2;
        List<Group> groups = new()
                             {
                                 new(this._guids[0], this._groupNames[0], this._players[..2]),
                                 new(this._guids[1], this._groupNames[1], this._players[2..]),
                             };
        List<Team> teams = new()
                           {
                               new(this._guids[0], this._teamNames[0], this._players[..2]),
                               new(this._guids[1], this._teamNames[1], this._players[2..]),
                           };

        // Act
        var tournament = new TeamTournament(id,
                                            name,
                                            drawSystem,
                                            coefficients,
                                            maxTour,
                                            createdAt,
                                            false)
                         {
                             Teams = new HashSet<Team>(teams),
                             GamePairs =
                                 new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(),
                             Groups = new HashSet<Group>(groups),
                         };
        ;

        // Assert
        Assert.Equal(new Id<Guid>(guid), tournament.Id);
        Assert.Equal(new Name("Test"),   tournament.Name);
        Assert.Equal(DrawSystem.Swiss,   tournament.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz },
                     tournament.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.Equal(new TourNumber(7),        tournament.MaxTour);
        Assert.Equal(new TourNumber(1),        tournament.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     tournament.Groups);
        Assert.Equal(new List<Team>
                     {
                         new(this._guids[0], this._teamNames[0], this._players[..2]),
                         new(this._guids[1], this._teamNames[1], this._players[2..]),
                     },
                     tournament.Teams);
        Assert.Equal(TournamentKind.Team, tournament.Kind);
    }

    [Fact]
    public void CreateTeamTournament_RoundRobin_Should_SetAllProps1()
    {
        // Arrange
        var                   guid         = Guid.NewGuid();
        Id<Guid>              id           = guid;
        Name                  name         = "Test";
        const DrawSystem      drawSystem   = DrawSystem.RoundRobin;
        List<DrawCoefficient> coefficients = new();
        DateOnly              createdAt    = new(2021, 1, 1);
        TourNumber            maxTour      = 1;
        TourNumber            currentTour  = 1;
        List<Group>           groups       = new();
        List<Team>            teams        = new();

        // Act
        var tournament = new TeamTournament(id,
                                            name,
                                            drawSystem,
                                            coefficients,
                                            maxTour,
                                            createdAt,
                                            false)
                         {
                             Teams = new HashSet<Team>(teams),
                             GamePairs =
                                 new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(),
                             Groups = new HashSet<Group>(groups),
                         };
        ;

        // Assert
        Assert.Equal(new Id<Guid>(guid),                 tournament.Id);
        Assert.Equal(new Name("Test"),                   tournament.Name);
        Assert.Equal(DrawSystem.RoundRobin,              tournament.System);
        Assert.Equal(new List<DrawCoefficient>(),        tournament.Coefficients);
        Assert.Equal(new TourNumber(1),                  tournament.MaxTour);
        Assert.Equal(new DateOnly(2021, 1, 1),           tournament.CreatedAt);
        Assert.Equal(new TourNumber(1),                  tournament.CurrentTour);
        Assert.Equal(new List<Group>(),                  tournament.Groups);
        Assert.Equal(new List<Team>(),                   tournament.Teams);
        Assert.Equal(TournamentKind.Team, tournament.Kind);
    }

    [Fact]
    public void CreateTeamTournament_RoundRobin_Should_SetAllProps2()
    {
        // Arrange
        var              guid       = Guid.NewGuid();
        Id<Guid>         id         = guid;
        Name             name       = "Test";
        const DrawSystem drawSystem = DrawSystem.RoundRobin;
        List<DrawCoefficient> coefficients =
            new() { DrawCoefficient.Berger, DrawCoefficient.SimpleBerger };
        DateOnly   createdAt   = new(2021, 1, 1);
        TourNumber maxTour     = 7;
        TourNumber currentTour = 2;
        List<Group> groups = new()
                             {
                                 new(this._guids[0], this._groupNames[0], this._players[..2]),
                                 new(this._guids[1], this._groupNames[1], this._players[2..]),
                             };
        List<Team> teams = new()
                           {
                               new(this._guids[0], this._teamNames[0], this._players[..2]),
                               new(this._guids[1], this._teamNames[1], this._players[2..]),
                           };

        // Act
        var tournament = new TeamTournament(id,
                                            name,
                                            drawSystem,
                                            coefficients,
                                            maxTour,
                                            createdAt,
                                            false)
                         {
                             Teams     = new HashSet<Team>(teams),
                             GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(),
                             Groups    = new HashSet<Group>(groups),
                         };

        // Assert
        Assert.Equal(new Id<Guid>(guid),    tournament.Id);
        Assert.Equal(new Name("Test"),      tournament.Name);
        Assert.Equal(DrawSystem.RoundRobin, tournament.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Berger, DrawCoefficient.SimpleBerger },
                     tournament.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.Equal(new TourNumber(7),        tournament.MaxTour);
        Assert.Equal(new TourNumber(1),        tournament.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     tournament.Groups);
        Assert.Equal(new List<Team>
                     {
                         new(this._guids[0], this._teamNames[0], this._players[..2]),
                         new(this._guids[1], this._teamNames[1], this._players[2..]),
                     },
                     tournament.Teams);
        Assert.Equal(TournamentKind.Team, tournament.Kind);
    }

    #endregion Create Team Tournament

    #region Create Single-Team Tournament

    [Fact]
    public void CreateSingleTeamTournament_Swiss_Should_SetAllProps1()
    {
        // Arrange
        var                   guid         = Guid.NewGuid();
        Id<Guid>              id           = guid;
        Name                  name         = "Test";
        const DrawSystem      drawSystem   = DrawSystem.Swiss;
        List<DrawCoefficient> coefficients = new();
        DateOnly              createdAt    = new(2021, 1, 1);
        TourNumber            maxTour      = 1;
        TourNumber            currentTour  = 1;
        List<Group>           groups       = new();
        List<Team>            teams        = new();

        // Act
        var tournament = new SingleTeamTournament(id,
                                                  name,
                                                  drawSystem,
                                                  coefficients,
                                                  maxTour,
                                                  createdAt,
                                                  false)
                         {
                             Teams     = new HashSet<Team>(teams),
                             GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                             Groups    = new HashSet<Group>(groups),
                         };

        // Assert
        Assert.Equal(new Id<Guid>(guid),                       tournament.Id);
        Assert.Equal(new Name("Test"),                         tournament.Name);
        Assert.Equal(DrawSystem.Swiss,                         tournament.System);
        Assert.Equal(new List<DrawCoefficient>(),              tournament.Coefficients);
        Assert.Equal(new TourNumber(1),                        tournament.MaxTour);
        Assert.Equal(new DateOnly(2021, 1, 1),                 tournament.CreatedAt);
        Assert.Equal(new TourNumber(1),                        tournament.CurrentTour);
        Assert.Equal(new List<Group>(),                        tournament.Groups);
        Assert.Equal(new List<Team>(),                         tournament.Teams);
        Assert.Equal(TournamentKind.SingleTeam, tournament.Kind);
    }

    [Fact]
    public void CreateSingleTeamTournament_Swiss_Should_SetAllProps2()
    {
        // Arrange
        var              guid       = Guid.NewGuid();
        Id<Guid>         id         = guid;
        Name             name       = "Test";
        const DrawSystem drawSystem = DrawSystem.Swiss;
        List<DrawCoefficient> coefficients =
            new() { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz };
        DateOnly   createdAt   = new(2021, 1, 1);
        TourNumber maxTour     = 7;
        TourNumber currentTour = 2;
        List<Group> groups = new()
                             {
                                 new(this._guids[0], this._groupNames[0], this._players[..2]),
                                 new(this._guids[1], this._groupNames[1], this._players[2..]),
                             };
        List<Team> teams = new()
                           {
                               new(this._guids[0], this._teamNames[0], this._players[..2]),
                               new(this._guids[1], this._teamNames[1], this._players[2..]),
                           };

        // Act
        var tournament = new SingleTeamTournament(id,
                                                  name,
                                                  drawSystem,
                                                  coefficients,
                                                  maxTour,
                                                  createdAt,
                                                  false)
                         {
                             Teams     = new HashSet<Team>(teams),
                             GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                             Groups    = new HashSet<Group>(groups),
                         };

        // Assert
        Assert.Equal(new Id<Guid>(guid), tournament.Id);
        Assert.Equal(new Name("Test"),   tournament.Name);
        Assert.Equal(DrawSystem.Swiss,   tournament.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz },
                     tournament.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.Equal(new TourNumber(7),        tournament.MaxTour);
        Assert.Equal(new TourNumber(1),        tournament.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     tournament.Groups);
        Assert.Equal(new List<Team>
                     {
                         new(this._guids[0], this._teamNames[0], this._players[..2]),
                         new(this._guids[1], this._teamNames[1], this._players[2..]),
                     },
                     tournament.Teams);
        Assert.Equal(TournamentKind.SingleTeam, tournament.Kind);
    }

    [Fact]
    public void CreateSingleTeamTournament_RoundRobin_Should_SetAllProps1()
    {
        // Arrange
        var                   guid         = Guid.NewGuid();
        Id<Guid>              id           = guid;
        Name                  name         = "Test";
        const DrawSystem      drawSystem   = DrawSystem.RoundRobin;
        List<DrawCoefficient> coefficients = new();
        DateOnly              createdAt    = new(2021, 1, 1);
        TourNumber            maxTour      = 1;
        TourNumber            currentTour  = 1;
        List<Group>           groups       = new();
        List<Team>            teams        = new();

        // Act
        var tournament = new SingleTeamTournament(id,
                                                  name,
                                                  drawSystem,
                                                  coefficients,
                                                  maxTour,
                                                  createdAt,
                                                  false)
                         {
                             Teams     = new HashSet<Team>(teams),
                             GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                             Groups    = new HashSet<Group>(groups),
                         };

        // Assert
        Assert.Equal(new Id<Guid>(guid),                       tournament.Id);
        Assert.Equal(new Name("Test"),                         tournament.Name);
        Assert.Equal(DrawSystem.RoundRobin,                    tournament.System);
        Assert.Equal(new List<DrawCoefficient>(),              tournament.Coefficients);
        Assert.Equal(new TourNumber(1),                        tournament.MaxTour);
        Assert.Equal(new DateOnly(2021, 1, 1),                 tournament.CreatedAt);
        Assert.Equal(new TourNumber(1),                        tournament.CurrentTour);
        Assert.Equal(new List<Group>(),                        tournament.Groups);
        Assert.Equal(new List<Team>(),                         tournament.Teams);
        Assert.Equal(TournamentKind.SingleTeam, tournament.Kind);
    }

    [Fact]
    public void CreateSingleTeamTournament_RoundRobin_Should_SetAllProps2()
    {
        // Arrange
        var              guid       = Guid.NewGuid();
        Id<Guid>         id         = guid;
        Name             name       = "Test";
        const DrawSystem drawSystem = DrawSystem.RoundRobin;
        List<DrawCoefficient> coefficients =
            new() { DrawCoefficient.Berger, DrawCoefficient.SimpleBerger };
        DateOnly   createdAt   = new(2021, 1, 1);
        TourNumber maxTour     = 7;
        TourNumber currentTour = 2;
        List<Group> groups = new()
                             {
                                 new(this._guids[0], this._groupNames[0], this._players[..2]),
                                 new(this._guids[1], this._groupNames[1], this._players[2..]),
                             };
        List<Team> teams = new()
                           {
                               new(this._guids[0], this._teamNames[0], this._players[..2]),
                               new(this._guids[1], this._teamNames[1], this._players[2..]),
                           };

        // Act
        var tournament = new SingleTeamTournament(id,
                                                  name,
                                                  drawSystem,
                                                  coefficients,
                                                  maxTour,
                                                  createdAt,
                                                  false)
                         {
                             Teams     = new HashSet<Team>(teams),
                             GamePairs = new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(),
                             Groups    = new HashSet<Group>(groups),
                         };

        // Assert
        Assert.Equal(new Id<Guid>(guid),    tournament.Id);
        Assert.Equal(new Name("Test"),      tournament.Name);
        Assert.Equal(DrawSystem.RoundRobin, tournament.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Berger, DrawCoefficient.SimpleBerger },
                     tournament.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), tournament.CreatedAt);
        Assert.Equal(new TourNumber(7),        tournament.MaxTour);
        Assert.Equal(new TourNumber(1),        tournament.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     tournament.Groups);
        Assert.Equal(new List<Team>
                     {
                         new(this._guids[0], this._teamNames[0], this._players[..2]),
                         new(this._guids[1], this._teamNames[1], this._players[2..]),
                     },
                     tournament.Teams);
        Assert.Equal(TournamentKind.SingleTeam, tournament.Kind);
    }

    #endregion Create Single-Team Tournament
}
