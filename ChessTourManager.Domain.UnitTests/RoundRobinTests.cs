using System.Collections.Generic;
using System.Linq;
using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Delete;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Algorithms;

namespace ChessTourManager.Domain.UnitTests;

[TestFixture]
public class RoundRobinTests
{
    // =====Test cases=====
    // 1. The draw of the first round.
    // 1.1. There are no active players.
    // 1.1.1. The list of players is empty.
    // 1.1.2. Only inactive players (one).
    // 1.1.3. Only inactive players (even number).
    // 1.1.4. Only inactive players (odd number).
    // 1.2. Active player alone.
    // 1.2.1. One active.
    // 1.2.2. One active and one inactive.
    // 1.2.3. One active and even number of inactive.
    // 1.2.4. One active and odd number of inactive.
    // 1.3. Active players are even.
    // 1.3.1. There are no inactive ones.
    // 1.3.2. There is one inactive one.
    // 1.3.3. There is an even number of inactive ones.
    // 1.3.4. There is an odd number of inactive ones.
    // 1.4. Active players are an odd number.
    // 1.4.1. There are no inactive ones.
    // 1.4.2. There is one inactive one.
    // 1.4.3. There is an even number of inactive ones.
    // 1.4.4. There is an odd number of inactive ones.
    // 2. The draw is not the first round.
    // 2.1. There are no active players.
    // 2.2. Active player alone.
    // 2.3. Active players are even.
    // 2.4. Active players are an odd number. */

    private const    int                OrgId    = 16;
    private const    int                TourId   = 53;
    private readonly ChessTourContext   _context = new();
    private          Tournament?        _tournament;
    private          IDrawingAlgorithm? _roundAlgorithm;

    // 1.1.1 The list of players is empty.
    [Test]
    public void DrawFirstRound_WhenNoPlayers_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.IsEmpty(result);

        this.ClearPlayers();
    }

    // 1.1.2. Only inactive players (one).
    [Test]
    public void DrawFirstRound_WhenOnlyOneInactivePlayer_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();


        this.AddPlayers(new[] { ("Петров", "Петр") }, false);
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(1, this._tournament.Players.Count);
        Assert.AreEqual(0, this._tournament.Players.Count(p => p.IsActive ?? false));
        Assert.IsEmpty(result);

        this.ClearPlayers();
    }

    // 1.1.3. Only inactive players (even number).
    [Test]
    public void DrawFirstRound_WhenOnlyEvenNumberOfInactivePlayers_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        (string, string)[] players =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Смирнов", "Сергей")
        };
        this.AddPlayers(players, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(4, this._tournament.Players.Count);
        Assert.AreEqual(0, this._tournament.Players.Count(p => p.IsActive ?? false));
        Assert.IsEmpty(result);

        this.ClearPlayers();
    }

    // 1.1.4. Only inactive players (odd number).
    [Test]
    public void DrawFirstRound_WhenOnlyOddNumberOfInactivePlayers_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        (string, string)[] players =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Смирнов", "Сергей"),
            ("Кузнецов", "Андрей")
        };
        this.AddPlayers(players, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(5, this._tournament.Players.Count);
        Assert.AreEqual(0, this._tournament.Players.Count(p => p.IsActive ?? false));
        Assert.IsEmpty(result);

        this.ClearPlayers();
    }

    // 1.2.1. One active player.
    [Test]
    public void DrawFirstRound_WhenOnePlayer_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        this.AddPlayers(new[] { ("Петров", "Петр") });
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.IsEmpty(result);
        Assert.AreEqual(1, this._tournament.Players.Count);

        this.ClearPlayers();
    }

    // 1.2.2. One active and one inactive player.
    [Test]
    public void DrawFirstRound_WhenOneActiveAndOneInactivePlayer_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        this.AddPlayers(new[] { ("Петров", "Петр") });
        this.AddPlayers(new[] { ("Сидоров", "Сидор") }, false);
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.IsEmpty(result);
        Assert.AreEqual(2, this._tournament.Players.Count);
        Assert.AreEqual(1, this._tournament.Players.Count(p => p.IsActive ?? false));

        this.ClearPlayers();
    }

    // 1.2.3. One active and even number of inactive players (4).
    [Test]
    public void DrawFirstRound_WhenOneActiveAndEvenNumberOfInactivePlayers_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        this.AddPlayers(new[] { ("Петров", "Петр") });
        (string, string)[] players =
        {
            ("Иванов", "Иван"), ("Сидоров", "Сидор"),
            ("Смирнов", "Сергей"), ("Кузнецов", "Андрей")
        };
        this.AddPlayers(players, false);
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.IsEmpty(result);
        Assert.AreEqual(5, this._tournament.Players.Count);
        Assert.AreEqual(1, this._tournament.Players.Count(p => p.IsActive ?? false));

        this.ClearPlayers();
    }

    // 1.2.4. One active and odd number of inactive players (5).
    [Test]
    public void DrawFirstRound_WhenOneActiveAndOddNumberOfInactivePlayers_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        this.AddPlayers(new[] { ("Петров", "Петр") });
        (string, string)[] players =
        {
            ("Иванов", "Иван"), ("Сидоров", "Сидор"),
            ("Смирнов", "Сергей"), ("Кузнецов", "Андрей"),
            ("Петров", "Андрей")
        };
        this.AddPlayers(players, false);
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.IsEmpty(result);
        Assert.AreEqual(6, this._tournament.Players.Count);
        Assert.AreEqual(1, this._tournament.Players.Count(p => p.IsActive ?? false));

        this.ClearPlayers();
    }

    // 1.3.1. Even number of active players (4).
    [Test]
    public void DrawFirstRound_WhenEvenNumberOfPlayers_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей")
        };
        this.AddPlayers(playerNames);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(4, this._tournament.Players.Count);
        Assert.AreEqual(2, result.Count());

        this.ClearPlayers();
    }

    // 1.3.2. Active players are even number (4) and There is one inactive one.
    [Test]
    public void DrawFirstRound_WhenEvenNumberOfPlayersAndOneInactivePlayer_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей")
        };
        this.AddPlayers(playerNames);
        this.AddPlayers(new[] { ("Алексеев", "Алексей") }, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(5, this._tournament.Players.Count);
        Assert.AreEqual(2, result.Count());

        this.ClearPlayers();
    }

    // 1.3.3. Active players are even number (4) and inactive players are even number (4).
    [Test]
    public void DrawFirstRound_WhenEvenNumberOfPlayersAndEvenNumberOfInactivePlayers_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей")
        };
        this.AddPlayers(playerNames);
        (string, string)[] inactivePlayers =
        {
            ("Алексеев", "Алексей"), ("Андреев", "Андрей"),
            ("Антонов", "Антон"), ("Артемов", "Артем")
        };
        this.AddPlayers(inactivePlayers, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(8, this._tournament.Players.Count);
        Assert.AreEqual(2, result.Count());

        this.ClearPlayers();
    }

    // 1.3.4. Active players are even number (4) and inactive players are odd number (5).
    [Test]
    public void DrawFirstRound_WhenEvenNumberOfPlayersAndOddNumberOfInactivePlayers_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();

        (string, string)[] activePlayers =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей")
        };
        this.AddPlayers(activePlayers);
        (string, string)[] inactivePlayers =
        {
            ("Алексеев", "Алексей"), ("Андреев", "Андрей"),
            ("Антонов", "Антон"), ("Артемов", "Артем"),
            ("Борисов", "Борис")
        };
        this.AddPlayers(inactivePlayers, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(9, this._tournament.Players.Count);
        Assert.AreEqual(2, result.Count());

        this.ClearPlayers();
    }

    // 1.4.1 Odd number of active players (5).
    [Test]
    public void DrawFirstRound_WhenOddNumberOfPlayers_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
            ("Алексеев", "Алексей")
        };
        this.AddPlayers(playerNames);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(5, this._tournament.Players.Count);
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        this.ClearPlayers();
    }

    // 1.4.2. Odd number of active players (5) and one inactive player.
    [Test]
    public void DrawFirstRound_WhenOddNumberOfPlayersAndOneInactivePlayer_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
            ("Алексеев", "Алексей")
        };
        this.AddPlayers(playerNames);
        this.AddPlayers(new[] { ("Андреев", "Андрей") }, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(6, this._tournament.Players.Count);
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        this.ClearPlayers();
    }

    // 1.4.3. Odd number of active players (5) and two inactive players.
    [Test]
    public void DrawFirstRound_WhenOddNumberOfPlayersAndTwoInactivePlayers_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
            ("Алексеев", "Алексей")
        };
        this.AddPlayers(playerNames);
        this.AddPlayers(new[] { ("Андреев", "Андрей"), ("Антонов", "Антон") }, false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(7, this._tournament.Players.Count);
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        this.ClearPlayers();
    }

    // 1.4.4. Odd number of active players (5) and three inactive players.
    [Test]
    public void DrawFirstRound_WhenOddNumberOfPlayersAndThreeInactivePlayers_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
            ("Алексеев", "Алексей")
        };
        this.AddPlayers(playerNames);
        this.AddPlayers(new[]
                        {
                            ("Андреев", "Андрей"), ("Антонов", "Антон"),
                            ("Артемов", "Артем")
                        },
                        false);

        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);

        // Act.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);

        // Assert.
        Assert.AreEqual(8, this._tournament.Players.Count);
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        this.ClearPlayers();
    }

    // 2.1. No players in second round.
    [Test]
    public void DrawNotFirstRound_WhenNoPlayers_ReturnsEmptyList()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей")
        };
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);
        this.AddPlayers(playerNames);

        // Act.
        // Start first round.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);
        Assert.AreEqual(2, result.Count());

        // Make all players inactive.
        this.SetAllActivity(false);

        // Start second round.
        result = this._roundAlgorithm.StartNewTour(1);

        // Assert.
        Assert.IsEmpty(result);

        this.ClearPlayers();
    }

    // 2.2. One player in second round.
    [Test]
    public void DrawNotFirstRound_WhenOnePlayer_ReturnsCorrectPair()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей")
        };
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);
        this.AddPlayers(playerNames);

        // Act.
        // Start first round.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);
        Assert.AreEqual(2, result.Count());

        // Make all players inactive except one.
        this.SetAllActivity(false);
        this._tournament.Players.First().IsActive = true;

        // Start second round.
        result = this._roundAlgorithm.StartNewTour(1);

        // Assert.
        Assert.AreEqual(0, result.Count());

        this.ClearPlayers();
    }

    // 2.3. 7 active in first round and then 4 active in second round.
    [Test]
    public void DrawNotFirstRound_When7ActiveInFirstRoundAnd4ActiveInSecondRound_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
            ("Алексеев", "Алексей"), ("Андреев", "Андрей"),
            ("Антонов", "Антон")
        };
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);
        this.AddPlayers(playerNames);

        // Act.
        // Start first round.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);
        Assert.AreEqual(4, result.Count());

        // Check if there is one player with -1 in pair.
        Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        // Make 3 players inactive.
        this._tournament.Players.Take(3).ToList().ForEach(p => p.IsActive = false);

        // Start second round.
        result = this._roundAlgorithm.StartNewTour(1);

        // Assert.
        Assert.AreEqual(2, result.Count());
        // Check if no player with -1 in pair.
        Assert.IsFalse(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        this.ClearPlayers();
    }

    // 2.4. 8 active in first round and then 5 active in second round.
    [Test]
    public void DrawNotFirstRound_When8ActiveInFirstRoundAnd5ActiveInSecondRound_ReturnsCorrectPairs()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                         out List<Tournament?>? tournaments);
        this._tournament = tournaments.Single(t => t.Id == TourId);
        // Arrange.
        this.ClearPlayers();
        (string, string)[] playerNames =
        {
            ("Петров", "Петр"), ("Иванов", "Иван"),
            ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
            ("Алексеев", "Алексей"), ("Андреев", "Андрей"),
            ("Антонов", "Антон"), ("Артемов", "Артем")
        };
        this._roundAlgorithm = IDrawingAlgorithm.Initialize(this._context, this._tournament);
        this.AddPlayers(playerNames);

        // Act.
        // Start first round.
        IEnumerable<(int, int)> result = this._roundAlgorithm.StartNewTour(0);
        Assert.AreEqual(4, result.Count());

        // Check if there is no player with -1 in pair.
        Assert.IsFalse(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        // Make 3 players inactive.
        this._tournament.Players.Take(3).ToList().ForEach(p => p.IsActive = false);

        // Start second round.
        result = this._roundAlgorithm.StartNewTour(1);

        // Assert.
        Assert.AreEqual(3, result.Count());
        // Check if there is a player with -1 in pair.
        Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));

        this.ClearPlayers();
    }

    private void SetAllActivity(bool isActive)
    {
        foreach (Player? player in this._tournament.Players)
        {
            player.IsActive = isActive;
        }
    }

    private void AddPlayers(IEnumerable<(string, string)> playerNames, bool isActive = true)
    {
        foreach ((string, string) fullName in playerNames)
        {
            IInsertQueries.CreateInstance(this._context)
                          .TryAddPlayer(out Player _,
                                        TourId,         OrgId,
                                        fullName.Item1, fullName.Item2,
                                        isActive: isActive);
        }
    }

    private void ClearPlayers()
    {
        IGetQueries.CreateInstance(this._context)
                   .TryGetPlayers(OrgId, TourId,
                                  out List<Player>? players);
        if (players is not null)
        {
            IEnumerable<Player> playersEnum = players.ToArray();
            for (var i = 0; i < playersEnum.Count(); i++)
            {
                Player player = playersEnum.ElementAt(i);
                IDeleteQueries.CreateInstance(this._context).TryDeletePlayer(player);
            }
        }
    }
}
