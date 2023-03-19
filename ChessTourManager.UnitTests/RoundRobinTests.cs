using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.Queries.Delete;
using ChessTourManager.DataAccess.Queries.Get;
using ChessTourManager.DataAccess.Queries.Insert;
using ChessTourManager.Domain.Algorithms;

namespace ChessTourManager.UnitTests
{
    [TestFixture]
    public class RoundRobinTests
    {
        // 1.	Drawing the first round.
        // 1.1.	No players.
        // 1.2.	One player.
        // 1.3.	Even number of players.
        // 1.4.	Odd number of players.
        // 2.	Drawing not the first round.
        // 2.1.	No players.
        // 2.2.	One player.
        // 2.3.	Even number of players.
        // 2.4.	Odd number of players.
        // 3.	Drawing the last round.
        // 3.1.	No players.
        // 3.2.	One player.
        // 3.3.	Even number of players.
        // 3.4.	Odd number of players.
        private const    int              OrgId    = 16;
        private const    int              TourId   = 53;
        private readonly ChessTourContext _context = new();
        private          Tournament       _tournament;
        private          IRoundRobin      _roundAlgorithm;

        #region 1. Drawing the first round.

        // 1.1. No players.
        [Test]
        public void DrawFirstRound_WhenNoPlayers_ReturnsEmptyList()
        {
            IGetQueries.CreateInstance(_context)
                       .TryGetTournamentsWithTeamsAndPlayers(OrgId,
                                                             out IEnumerable<Tournament>? tournaments);
            _tournament = tournaments.Single(t => t.TournamentId == TourId);
            // Arrange.
            List<Player> players = _tournament.Players.ToList();
            foreach (Player player in players)
            {
                IDeleteQueries.CreateInstance(_context).TryDeletePlayer(player);
            }
            _roundAlgorithm = IRoundRobin.Initialize(_context, _tournament);

            // Act.
            IEnumerable<(int, int)> result = _roundAlgorithm.StartNewTour(0);

            // Assert.
            Assert.IsEmpty(result);
        }

        // 1.2. One player.
        [Test]
        public void DrawFirstRound_WhenOnePlayer_ReturnsEmptyList()
        {
            IGetQueries.CreateInstance(_context)
                       .TryGetTournamentsWithTeamsAndPlayers(OrgId, out IEnumerable<Tournament>? tournaments);
            _tournament = tournaments.Single(t => t.TournamentId == TourId);
            // Arrange.
            List<Player> players = _tournament.Players.ToList();
            foreach (Player player in players)
            {
                IDeleteQueries.CreateInstance(_context).TryDeletePlayer(player);
            }

            IInsertQueries.CreateInstance(_context)
                          .TryAddPlayer(out Player _,
                                        TourId,   OrgId,
                                        "Петров", "Петр");
            _roundAlgorithm = IRoundRobin.Initialize(_context, _tournament);

            // Act.
            IEnumerable<(int, int)> result = _roundAlgorithm.StartNewTour(0);

            // Assert.
            Assert.IsEmpty(result);
            Assert.AreEqual(1, _tournament.Players.Count);
        }

        // 1.3. Even number of players (4).
        [Test]
        public void DrawFirstRound_WhenEvenNumberOfPlayers_ReturnsCorrectPairs()
        {
            IGetQueries.CreateInstance(_context)
                       .TryGetTournamentsWithTeamsAndPlayers(OrgId, out IEnumerable<Tournament>? tournaments);
            _tournament = tournaments.Single(t => t.TournamentId == TourId);
            // Arrange.
            List<Player> players = _tournament.Players.ToList();
            foreach (Player player in players)
            {
                IDeleteQueries.CreateInstance(_context).TryDeletePlayer(player);
            }

            (string, string)[] playerNames =
            {
                ("Петров", "Петр"),
                ("Иванов", "Иван"),
                ("Сидоров", "Сидор"),
                ("Сергеев", "Сергей")
            };
            foreach ((string, string) fullName in playerNames)
            {
                IInsertQueries.CreateInstance(_context)
                              .TryAddPlayer(out Player _,
                                            TourId,         OrgId,
                                            fullName.Item1, fullName.Item2);
            }

            _roundAlgorithm = IRoundRobin.Initialize(_context, _tournament);

            // Act.
            IEnumerable<(int, int)> result = _roundAlgorithm.StartNewTour(0);

            // Assert.
            Assert.AreEqual(4, _tournament.Players.Count);
            Assert.AreEqual(2, result.Count());
        }

        // 1.4. Odd number of players (5).
        [Test]
        public void DrawFirstRound_WhenOddNumberOfPlayers_ReturnsCorrectPairs()
        {
            IGetQueries.CreateInstance(_context)
                       .TryGetTournamentsWithTeamsAndPlayers(OrgId, out IEnumerable<Tournament>? tournaments);
            _tournament = tournaments.Single(t => t.TournamentId == TourId);
            // Arrange.
            List<Player> players = _tournament.Players.ToList();
            foreach (Player player in players)
            {
                IDeleteQueries.CreateInstance(_context).TryDeletePlayer(player);
            }
            (string, string)[] playerNames = { ("Петров", "Петр"), ("Иванов", "Иван"),
                                                 ("Сидоров", "Сидор"), ("Сергеев", "Сергей"),
                                                 ("Алексеев", "Алексей") };
            foreach ((string, string) fullName in playerNames)
            {
                IInsertQueries.CreateInstance(_context)
                              .TryAddPlayer(out Player _,
                                            TourId,         OrgId,
                                            fullName.Item1, fullName.Item2);
            }
            _roundAlgorithm = IRoundRobin.Initialize(_context, _tournament);

            // Act.
            IEnumerable<(int, int)> result = _roundAlgorithm.StartNewTour(0);

            // Assert.
            Assert.AreEqual(5, _tournament.Players.Count);
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Any(pair => pair.Item1 == -1 || pair.Item2 == -1));
        }

        #endregion
    }
}
