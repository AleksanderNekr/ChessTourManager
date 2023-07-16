using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Tests;

public class TeamTournamentGetPlayersPairsTests
{
    [Fact]
    public void Pairs_WhenEqual_RightResults()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        GamePair<Player> pair = new(new Player(id1, "Player 1"),
                                    new Player(id2, "Player 2"),
                                    GameResult.Draw);

        // Act
        GamePair<Player> pair2 = new(new Player(id1, "Player 1"),
                                     new Player(id2, "Player 2"),
                                     GameResult.Draw);

        // Assert
        Assert.Equal(pair, pair2);
    }

    [Fact]
    public void Pairs_WhenNotEqual_WrongResults()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        GamePair<Player> pair = new(new Player(id1, "Player 1"),
                                    new Player(id2, "Player 2"),
                                    GameResult.Draw);

        // Act
        GamePair<Player> pair2 = new(new Player(id1, "Player 1"),
                                     new Player(id2, "Player 2"),
                                     GameResult.WhiteWin);

        // Assert
        Assert.NotEqual(pair, pair2);
    }

    [Fact]
    public void GetPlayersPairs_WhenCalled_ReturnsPairs()
    {
        // Arrange
        Team team1 = new(Guid.NewGuid(),
                         "Team1",
                         new[]
                         {
                             new Player(Guid.NewGuid(), "Player 1"),
                             new Player(Guid.NewGuid(), "Player 2")
                         });
        Team team2 = new(Guid.NewGuid(),
                         "Team2",
                         new[]
                         {
                             new Player(Guid.NewGuid(), "Player 3"),
                             new Player(Guid.NewGuid(), "Player 4")
                         });
        Team team3 = new(Guid.NewGuid(),
                         "Team3",
                         new[]
                         {
                             new Player(Guid.NewGuid(), "Player 5"),
                             new Player(Guid.NewGuid(), "Player 6")
                         });

        Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> pairings = new()
                                                                        {
                                                                            {
                                                                                1,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team2)
                                                                                }
                                                                            },
                                                                            {
                                                                                2,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team3)
                                                                                }
                                                                            }
                                                                        };


        TeamTournament tournament = new(id: Guid.NewGuid(),
                                        name: "Tournament",
                                        drawSystem: DrawSystem.RoundRobin,
                                        coefficients: new[] { DrawCoefficient.Berger },
                                        maxTour: 4,
                                        createdAt: DateOnly.FromDateTime(DateTime.UtcNow),
                                        true)
                                    {
                                        Teams     = new HashSet<Team>(new[] { team1, team2, team3 }),
                                        GamePairs = pairings,
                                        Groups    = new HashSet<Group>()
                                    };

        // Act
        IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> pairs = tournament.GetPlayersPairings();

        // Assert
        Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>> expectedPairs =
            new()
            {
                {
                    1,
                    new HashSet<GamePair<Player>>
                    {
                        new(team1.Players.ToArray()[0], team2.Players.ToArray()[0]),
                        new(team1.Players.ToArray()[1], team2.Players.ToArray()[1])
                    }
                },
                {
                    2,
                    new HashSet<GamePair<Player>>
                    {
                        new(team1.Players.ToArray()[0], team3.Players.ToArray()[0]),
                        new(team1.Players.ToArray()[1], team3.Players.ToArray()[1])
                    }
                }
            };

        Assert.Equal(expectedPairs, pairs);
    }

    [Fact]
    public void GetPlayersPairs_WhenCalled_WrongResults()
    {
        // Arrange
        Team team1 = new(Guid.NewGuid(),
                         "Team1",
                         new[]
                         {
                             new Player(Guid.NewGuid(), "Player 1"),
                             new Player(Guid.NewGuid(), "Player 2")
                         });
        Team team2 = new(Guid.NewGuid(),
                         "Team2",
                         new[]
                         {
                             new Player(Guid.NewGuid(), "Player 3"),
                             new Player(Guid.NewGuid(), "Player 4")
                         });
        Team team3 = new(Guid.NewGuid(),
                         "Team3",
                         new[]
                         {
                             new Player(Guid.NewGuid(), "Player 5"),
                             new Player(Guid.NewGuid(), "Player 6")
                         });

        Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> pairings = new()
                                                                        {
                                                                            {
                                                                                1,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team2)
                                                                                }
                                                                            },
                                                                            {
                                                                                2,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team3, GameResult.WhiteWin)
                                                                                }
                                                                            }
                                                                        };


        TeamTournament tournament = new(id: Guid.NewGuid(),
                                        name: "Tournament",
                                        drawSystem: DrawSystem.RoundRobin,
                                        coefficients: new[] { DrawCoefficient.Berger },
                                        maxTour: 4,
                                        createdAt: DateOnly.FromDateTime(DateTime.UtcNow),
                                        true)
                                    {
                                        Teams     = new HashSet<Team>(new[] { team1, team2, team3 }),
                                        GamePairs = pairings,
                                        Groups    = new HashSet<Group>()
                                    };

        // Act
        IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair<Player>>> pairs = tournament.GetPlayersPairings();

        // Assert
        Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>> expectedPairs =
            new()
            {
                {
                    1,
                    new HashSet<GamePair<Player>>
                    {
                        new(team1.Players.ToArray()[0], team2.Players.ToArray()[0]),
                        new(team1.Players.ToArray()[1], team2.Players.ToArray()[1])
                    }
                },
                {
                    2,
                    new HashSet<GamePair<Player>>
                    {
                        new(team1.Players.ToArray()[0], team3.Players.ToArray()[0]),
                        new(team1.Players.ToArray()[1], team3.Players.ToArray()[1])
                    }
                }
            };

        Assert.NotEqual(expectedPairs, pairs);
    }
}
