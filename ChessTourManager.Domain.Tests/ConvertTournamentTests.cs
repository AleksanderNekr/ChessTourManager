using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.ValueObjects;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable MethodTooLong

namespace ChessTourManager.Domain.Tests;

[TestFixture]
public class ConvertTournamentTests
{
    [Test]
    public void ConvertFromSingleToSingle1()
    {
        // Arrange
        ArrangeFromSingle1(out Guid guid,                      out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                           out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                           out TourNumber currentTour,         out List<Group> groups, out bool allowInGroupGames,
                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt,
                                                                            allowInGroupGames, gamePairs);
        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertToSingle1(guid, result);
    }

    [Test]
    public void ConvertFromSingleToSingle2()
    {
        // Arrange
        ArrangeFromSingle2(out Guid guid,                      out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                           out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                           out TourNumber currentTour,         out List<Group> groups, out bool allowInGroupGames,
                           out Id<Guid>[] playerIds,           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, allowInGroupGames, gamePairs);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertToSingle2(guid, result, playerIds);
    }

    [Test]
    public void ConvertFromSingleToSingleTeam1()
    {
        // Arrange
        ArrangeFromSingle1(out Guid guid,                      out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                           out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                           out TourNumber currentTour,         out List<Group> groups, out bool allowInGroupGames,
                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt,
                                                                            allowInGroupGames, gamePairs);
        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        AssertToSingleTeam1(guid, result);
    }

    [Test]
    public void ConvertFromSingleToSingleTeam2()
    {
        // Arrange
        ArrangeFromSingle2(out Guid guid,                      out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                           out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                           out TourNumber currentTour,         out List<Group> groups, out bool allowInGroupGames,
                           out Id<Guid>[] playerIds,           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        SingleTournament tournament =
            TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour, currentTour, groups,
                                                  createdAt, allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();
        result.Teams.Add(new Team { Name = "Team1" });

        // Assert
        AssertToSingleTeam2(guid, result, playerIds);
    }

    [Test]
    public void ConvertFromSingleToTeam1()
    {
        // Arrange
        ArrangeFromSingle1(out Guid guid,                      out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                           out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                           out TourNumber currentTour,         out List<Group> groups, out bool allowInGroupGames,
                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt,
                                                                            allowInGroupGames, gamePairs);
        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        AssertToTeam1(guid, result);
    }

    [Test]
    public void ConvertFromSingleToTeam2()
    {
        // Arrange
        ArrangeFromSingle2(out Guid guid,                      out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                           out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                           out TourNumber currentTour,         out List<Group> groups, out bool allowInGroupGames,
                           out Id<Guid>[] playerIds,           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        SingleTournament tournament = TournamentBase.CreateSingleTournament(id, name, drawSystem, coefficients, maxTour,
                                                                            currentTour, groups, createdAt,
                                                                            allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();
        result.Teams.Add(new Team { Name = "Team1" });

        // Assert
        AssertToTeam2(guid, result, playerIds);
    }

    [Test]
    public void ConvertFromSingleTeamToSingle1()
    {
        // Arrange
        Guid guid = ArrangeFromSingleTeam1(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                           out List<Coefficient> coefficients, out DateOnly createdAt,
                                           out TourNumber maxTour, out TourNumber currentTour, out List<Group> groups,
                                           out List<Team> teams, out bool allowInGroupGames,
                                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams,
            allowInGroupGames, gamePairs);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertToSingle1(guid, result);
    }

    [Test]
    public void ConvertFromSingleTeamToSingle2()
    {
        // Arrange
        Guid guid = ArrangeFromSingleTeam2(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                           out List<Coefficient> coefficients, out DateOnly createdAt,
                                           out TourNumber maxTour, out TourNumber currentTour, out List<Group> groups,
                                           out List<Team> teams, out bool allowInGroupGames, out Id<Guid>[] playerIds,
                                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament =
            TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour,
                                                                      groups, createdAt, teams,
                                                                      allowInGroupGames, gamePairs);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertToSingle2(guid, result, playerIds);
    }

    [Test]
    public void ConvertFromSingleTeamToTeam1()
    {
        // Arrange
        ArrangeFromSingleTeam1(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                               out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                               out TourNumber currentTour, out List<Group> groups, out List<Team> teams,
                               out bool allowInGroupGames, out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams,
            allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        AssertToTeam1(id, result);
    }

    [Test]
    public void ConvertFromSingleTeamToTeam2()
    {
        // Arrange
        ArrangeFromSingleTeam2(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                               out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                               out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                               out bool allowInGroupGames,         out Id<Guid>[] playerIds,
                               out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament =
            TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams,
                                                                      allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        AssertToTeam2(id, result, playerIds);
    }

    [Test]
    public void ConvertFromSingleTeamToSingleTeam1()
    {
        // Arrange
        ArrangeFromSingleTeam1(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                               out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                               out TourNumber currentTour, out List<Group> groups, out List<Team> teams,
                               out bool allowInGroupGames, out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients,
            maxTour, currentTour, groups, createdAt, teams, allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        AssertToSingleTeam1(id, result);
    }

    [Test]
    public void ConvertFromSingleTeamToSingleTeam2()
    {
        // Arrange
        ArrangeFromSingleTeam2(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                               out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                               out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                               out bool allowInGroupGames,         out Id<Guid>[] playerIds,
                               out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament =
            TournamentBase.CreateTeamTournament<SingleTeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                      currentTour, groups, createdAt, teams,
                                                                      allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        AssertToSingleTeam2(id, result, playerIds);
    }

    [Test]
    public void ConvertFromTeamToSingle1()
    {
        // Arrange
        ArrangeFromTeam1(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                         out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                         out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                         out bool allowInGroupGames,         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients,
                                                                             maxTour, currentTour, groups, createdAt,
                                                                             teams, allowInGroupGames, gamePairs);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertToSingle1(id, result);
    }

    [Test]
    public void ConvertFromTeamToSingle2()
    {
        // Arrange
        ArrangeFromTeam2(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                         out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                         out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                         out bool allowInGroupGames,         out Id<Guid>[] playerIds,
                         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament =
            TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                currentTour, groups, createdAt, teams,
                                                                allowInGroupGames, gamePairs);

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertToSingle2(id, result, playerIds);
    }

    [Test]
    public void ConvertFromTeamToSingleTeam1()
    {
        // Arrange
        ArrangeFromTeam1(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                         out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                         out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                         out bool allowInGroupGames,         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients,
                                                                             maxTour, currentTour, groups, createdAt,
                                                                             teams, allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        AssertToSingleTeam1(id, result);
    }

    [Test]
    public void ConvertFromTeamToSingleTeam2()
    {
        // Arrange
        ArrangeFromTeam2(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                         out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                         out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                         out bool allowInGroupGames,         out Id<Guid>[] playerIds,
                         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament =
            TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                currentTour, groups, createdAt, teams,
                                                                allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        AssertToSingleTeam2(id, result, playerIds);
    }

    [Test]
    public void ConvertFromTeamToTeam1()
    {
        // Arrange
        ArrangeFromTeam1(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                         out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                         out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                         out bool allowInGroupGames,         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament = TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients,
                                                                             maxTour, currentTour, groups, createdAt,
                                                                             teams, allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        AssertToTeam1(id, result);
    }

    [Test]
    public void ConvertFromTeamToTeam2()
    {
        // Arrange
        ArrangeFromTeam2(out Id<Guid> id,                    out Name name,          out DrawSystem drawSystem,
                         out List<Coefficient> coefficients, out DateOnly createdAt, out TourNumber maxTour,
                         out TourNumber currentTour,         out List<Group> groups, out List<Team> teams,
                         out bool allowInGroupGames,         out Id<Guid>[] playerIds,
                         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs);

        var tournament =
            TournamentBase.CreateTeamTournament<TeamTournament>(id, name, drawSystem, coefficients, maxTour,
                                                                currentTour, groups, createdAt, teams,
                                                                allowInGroupGames, gamePairs);

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        AssertToTeam2(id, result, playerIds);
    }

    private static void AssertToSingle1(Guid guid, SingleTournament result)
    {

        Assert.AreEqual(new Id<Guid>(guid),                              result.Id);
        Assert.AreEqual(new Name("Test"),                                result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,                           result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),                         result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                        result.CreatedAt);
        Assert.AreEqual(Kind.Single,                                     result.Kind);
        Assert.AreEqual(new TourNumber(1),                               result.MaxTour);
        Assert.AreEqual(new TourNumber(1),                               result.CurrentTour);
        Assert.AreEqual(new List<Group>(),                               result.Groups);
        Assert.AreEqual(false,                                           result.AllowInGroupGames);
        Assert.AreEqual(new Dictionary<TourNumber, HashSet<GamePair>>(), result.GamePairs);
    }

    private static void ArrangeFromSingle1(out Guid guid, out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                           out List<Coefficient> coefficients, out DateOnly createdAt,
                                           out TourNumber maxTour,
                                           out TourNumber currentTour, out List<Group> groups,
                                           out bool allowInGroupGames,
                                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
    {

        guid              = Guid.NewGuid();
        id                = guid;
        name              = "Test";
        drawSystem        = DrawSystem.RoundRobin;
        coefficients      = new List<Coefficient>();
        createdAt         = new DateOnly(2021, 1, 1);
        maxTour           = 1;
        currentTour       = 1;
        groups            = new List<Group>();
        allowInGroupGames = false;
        gamePairs         = new Dictionary<TourNumber, HashSet<GamePair>>();
    }


    private static void AssertToSingle2(Guid guid, SingleTournament result, Id<Guid>[] playerIds)
    {

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
        Assert.AreEqual(true, result.AllowInGroupGames);
        Assert.AreEqual(new Dictionary<TourNumber, HashSet<GamePair>>
                        {
                            {
                                new TourNumber(1), new HashSet<GamePair>
                                                   {
                                                       new(new Player(playerIds[0], "Player1"),
                                                           new Player(playerIds[1], "Player2")),
                                                       new(new Player(playerIds[2], "Player3"),
                                                           new Player(playerIds[3], "Player4")),
                                                   }
                            },
                            {
                                new TourNumber(2), new HashSet<GamePair>
                                                   {
                                                       new(new Player(playerIds[0], "Player1"),
                                                           new Player(playerIds[2], "Player3")),
                                                       new(new Player(playerIds[1], "Player2"),
                                                           new Player(playerIds[3], "Player4")),
                                                   }
                            },
                        }, result.GamePairs);
    }

    private static void ArrangeFromSingle2(out Guid guid, out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                           out List<Coefficient> coefficients, out DateOnly createdAt,
                                           out TourNumber maxTour,
                                           out TourNumber currentTour, out List<Group> groups,
                                           out bool allowInGroupGames,
                                           out Id<Guid>[] playerIds,
                                           out Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
    {

        guid              = Guid.NewGuid();
        id                = guid;
        name              = "Test";
        drawSystem        = DrawSystem.Swiss;
        coefficients      = new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        createdAt         = new DateOnly(2021, 1, 1);
        maxTour           = 7;
        currentTour       = 2;
        groups            = new List<Group> { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        allowInGroupGames = true;

        playerIds = new Id<Guid>[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        gamePairs = new Dictionary<TourNumber, HashSet<GamePair>>();
        gamePairs.Add(new TourNumber(1), new HashSet<GamePair>
                                         {
                                             new(new Player(playerIds[0], "Player1"),
                                                 new Player(playerIds[1], "Player2")),
                                             new(new Player(playerIds[2], "Player3"),
                                                 new Player(playerIds[3], "Player4")),
                                         });
        gamePairs.Add(new TourNumber(2), new HashSet<GamePair>
                                         {
                                             new(new Player(playerIds[0], "Player1"),
                                                 new Player(playerIds[2], "Player3")),
                                             new(new Player(playerIds[1], "Player2"),
                                                 new Player(playerIds[3], "Player4")),
                                         });
    }

    private static void AssertToSingleTeam1(Guid guid, SingleTeamTournament result)
    {

        Assert.AreEqual(new Id<Guid>(guid),                              result.Id);
        Assert.AreEqual(new Name("Test"),                                result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,                           result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),                         result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                        result.CreatedAt);
        Assert.AreEqual(Kind.SingleTeam,                                 result.Kind);
        Assert.AreEqual(new TourNumber(1),                               result.MaxTour);
        Assert.AreEqual(new TourNumber(1),                               result.CurrentTour);
        Assert.AreEqual(new List<Group>(),                               result.Groups);
        Assert.AreEqual(new List<Team>(),                                result.Teams);
        Assert.AreEqual(false,                                           result.AllowInGroupGames);
        Assert.AreEqual(new Dictionary<TourNumber, HashSet<GamePair>>(), result.GamePairs);
    }

    private static void AssertToSingleTeam2(Guid guid, SingleTeamTournament result, Id<Guid>[] playerIds)
    {

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
        Assert.AreEqual(true,                                        result.AllowInGroupGames);
        Assert.AreEqual(new Dictionary<TourNumber, HashSet<GamePair>>
                        {
                            {
                                new TourNumber(1), new HashSet<GamePair>
                                                   {
                                                       new(new Player(playerIds[0], "Player1"),
                                                           new Player(playerIds[1], "Player2")),
                                                       new(new Player(playerIds[2], "Player3"),
                                                           new Player(playerIds[3], "Player4")),
                                                   }
                            },
                            {
                                new TourNumber(2), new HashSet<GamePair>
                                                   {
                                                       new(new Player(playerIds[0], "Player1"),
                                                           new Player(playerIds[2], "Player3")),
                                                       new(new Player(playerIds[1], "Player2"),
                                                           new Player(playerIds[3], "Player4")),
                                                   }
                            },
                        }, result.GamePairs);
    }

    private static void AssertToTeam1(Guid guid, TeamTournament result)
    {

        Assert.AreEqual(new Id<Guid>(guid),                              result.Id);
        Assert.AreEqual(new Name("Test"),                                result.Name);
        Assert.AreEqual(DrawSystem.RoundRobin,                           result.DrawSystem);
        Assert.AreEqual(new List<Coefficient>(),                         result.Coefficients);
        Assert.AreEqual(new DateOnly(2021, 1, 1),                        result.CreatedAt);
        Assert.AreEqual(Kind.Team,                                       result.Kind);
        Assert.AreEqual(new TourNumber(1),                               result.MaxTour);
        Assert.AreEqual(new TourNumber(1),                               result.CurrentTour);
        Assert.AreEqual(new List<Group>(),                               result.Groups);
        Assert.AreEqual(new List<Team>(),                                result.Teams);
        Assert.AreEqual(false,                                           result.AllowInGroupGames);
        Assert.AreEqual(new Dictionary<TourNumber, HashSet<GamePair>>(), result.GamePairs);
    }

    private static void AssertToTeam2(Guid guid, TeamTournament result, Id<Guid>[] playerIds)
    {

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
        Assert.AreEqual(true,                                        result.AllowInGroupGames);
        Assert.AreEqual(new Dictionary<TourNumber, HashSet<GamePair>>
                        {
                            {
                                new TourNumber(1), new HashSet<GamePair>
                                                   {
                                                       new(new Player(playerIds[0], "Player1"),
                                                           new Player(playerIds[1], "Player2")),
                                                       new(new Player(playerIds[2], "Player3"),
                                                           new Player(playerIds[3], "Player4")),
                                                   }
                            },
                            {
                                new TourNumber(2), new HashSet<GamePair>
                                                   {
                                                       new(new Player(playerIds[0], "Player1"),
                                                           new Player(playerIds[2], "Player3")),
                                                       new(new Player(playerIds[1], "Player2"),
                                                           new Player(playerIds[3], "Player4")),
                                                   }
                            },
                        }, result.GamePairs);
    }

    private static Guid ArrangeFromSingleTeam1(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                               out List<Coefficient> coefficients,
                                               out DateOnly createdAt, out TourNumber maxTour, out TourNumber currentTour,
                                               out List<Group> groups, out List<Team> teams, out bool allowInGroupGames,
                                               out Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
    {

        var guid = Guid.NewGuid();
        id                = guid;
        name              = "Test";
        drawSystem        = DrawSystem.RoundRobin;
        coefficients      = new List<Coefficient>();
        createdAt         = new DateOnly(2021, 1, 1);
        maxTour           = 1;
        currentTour       = 1;
        groups            = new List<Group>();
        teams             = new List<Team>();
        allowInGroupGames = false;
        gamePairs         = new Dictionary<TourNumber, HashSet<GamePair>>();

        return guid;
    }

    private static Guid ArrangeFromTeam1(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                         out List<Coefficient> coefficients,
                                         out DateOnly createdAt, out TourNumber maxTour, out TourNumber currentTour,
                                         out List<Group> groups, out List<Team> teams, out bool allowInGroupGames,
                                         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
    {

        var guid = Guid.NewGuid();
        id                = guid;
        name              = "Test";
        drawSystem        = DrawSystem.RoundRobin;
        coefficients      = new List<Coefficient>();
        createdAt         = new DateOnly(2021, 1, 1);
        maxTour           = 1;
        currentTour       = 1;
        groups            = new List<Group>();
        teams             = new List<Team>();
        allowInGroupGames = false;
        gamePairs         = new Dictionary<TourNumber, HashSet<GamePair>>();

        return guid;
    }

    private static Guid ArrangeFromSingleTeam2(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                               out List<Coefficient> coefficients,
                                               out DateOnly createdAt, out TourNumber maxTour, out TourNumber currentTour,
                                               out List<Group> groups, out List<Team> teams, out bool allowInGroupGames,
                                               out Id<Guid>[] playerIds,
                                               out Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
    {

        var guid = Guid.NewGuid();
        id                = guid;
        name              = "Test";
        drawSystem        = DrawSystem.Swiss;
        coefficients      = new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        createdAt         = new DateOnly(2021, 1, 1);
        maxTour           = 7;
        currentTour       = 2;
        groups            = new List<Group> { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        teams             = new List<Team> { new Team { Name   = "Team1" } };
        allowInGroupGames = true;

        playerIds = new Id<Guid>[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        gamePairs = new Dictionary<TourNumber, HashSet<GamePair>>();
        gamePairs.Add(new TourNumber(1), new HashSet<GamePair>
                                         {
                                             new(new Player(playerIds[0], "Player1"),
                                                 new Player(playerIds[1], "Player2")),
                                             new(new Player(playerIds[2], "Player3"),
                                                 new Player(playerIds[3], "Player4")),
                                         });
        gamePairs.Add(new TourNumber(2), new HashSet<GamePair>
                                         {
                                             new(new Player(playerIds[0], "Player1"),
                                                 new Player(playerIds[2], "Player3")),
                                             new(new Player(playerIds[1], "Player2"),
                                                 new Player(playerIds[3], "Player4")),
                                         });

        return guid;
    }

    private static Guid ArrangeFromTeam2(out Id<Guid> id, out Name name, out DrawSystem drawSystem,
                                         out List<Coefficient> coefficients,
                                         out DateOnly createdAt, out TourNumber maxTour, out TourNumber currentTour,
                                         out List<Group> groups, out List<Team> teams, out bool allowInGroupGames,
                                         out Id<Guid>[] playerIds,
                                         out Dictionary<TourNumber, HashSet<GamePair>> gamePairs)
    {

        var guid = Guid.NewGuid();
        id                = guid;
        name              = "Test";
        drawSystem        = DrawSystem.Swiss;
        coefficients      = new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz };
        createdAt         = new DateOnly(2021, 1, 1);
        maxTour           = 7;
        currentTour       = 2;
        groups            = new List<Group> { new Group { Name = "Group1" }, new Group { Name = "Group2" } };
        teams             = new List<Team> { new Team { Name   = "Team1" } };
        allowInGroupGames = true;

        playerIds = new Id<Guid>[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        gamePairs = new Dictionary<TourNumber, HashSet<GamePair>>();
        gamePairs.Add(new TourNumber(1), new HashSet<GamePair>
                                         {
                                             new(new Player(playerIds[0], "Player1"),
                                                 new Player(playerIds[1], "Player2")),
                                             new(new Player(playerIds[2], "Player3"),
                                                 new Player(playerIds[3], "Player4")),
                                         });
        gamePairs.Add(new TourNumber(2), new HashSet<GamePair>
                                         {
                                             new(new Player(playerIds[0], "Player1"),
                                                 new Player(playerIds[2], "Player3")),
                                             new(new Player(playerIds[1], "Playerrr2"), //TODO
                                                 new Player(playerIds[3], "Player4")),
                                         });

        return guid;
    }
}
