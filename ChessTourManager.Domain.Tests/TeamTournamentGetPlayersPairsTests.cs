using ChessTourManager.Domain.Entities;
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

        GamePair<Player> pair = new(new Player(id1, "Player 1", Gender.Male, 2000),
                                    new Player(id2, "Player 2", Gender.Male, 2000),
                                    GameResult.Draw);

        // Act
        GamePair<Player> pair2 = new(new Player(id1, "Player 1", Gender.Male, 2000),
                                     new Player(id2, "Player 2", Gender.Male, 2000),
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

        GamePair<Player> pair = new(new Player(id1, "Player 1", Gender.Male, 2000),
                                    new Player(id2, "Player 2", Gender.Male, 2000),
                                    GameResult.Draw);

        // Act
        GamePair<Player> pair2 = new(new Player(id1, "Player 1", Gender.Male, 2000),
                                     new Player(id2, "Player 2", Gender.Male, 2000),
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
                         players: new[]
                                  {
                                      new Player(Guid.NewGuid(), "Player 1", Gender.Male, 2000),
                                      new Player(Guid.NewGuid(), "Player 2", Gender.Male, 2000),
                                  });
        Team team2 = new(Guid.NewGuid(),
                         "Team2",
                         players: new[]
                                  {
                                      new Player(Guid.NewGuid(), "Player 3", Gender.Male, 2000),
                                      new Player(Guid.NewGuid(), "Player 4", Gender.Male, 2000),
                                  });
        Team team3 = new(Guid.NewGuid(),
                         "Team3",
                         players: new[]
                                  {
                                      new Player(Guid.NewGuid(), "Player 5", Gender.Male, 2000),
                                      new Player(Guid.NewGuid(), "Player 6", Gender.Male, 2000),
                                  });

        Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> pairings = new()
                                                                        {
                                                                            {
                                                                                1,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team2),
                                                                                }
                                                                            },
                                                                            {
                                                                                2,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team3),
                                                                                }
                                                                            },
                                                                        };


        TeamTournament tournament = new(Guid.NewGuid(),
                                        "Tournament",
                                        DrawSystem.RoundRobin,
                                        new[] { DrawCoefficient.Berger },
                                        4,
                                        DateOnly.FromDateTime(DateTime.UtcNow),
                                        gamePairs: pairings)
                                    {
                                        Teams  = new HashSet<Team>(new[] { team1, team2, team3 }),
                                        Groups = new HashSet<Group>(),
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
                        new(team1.Players.ToArray()[1], team2.Players.ToArray()[1]),
                    }
                },
                {
                    2,
                    new HashSet<GamePair<Player>>
                    {
                        new(team1.Players.ToArray()[0], team3.Players.ToArray()[0]),
                        new(team1.Players.ToArray()[1], team3.Players.ToArray()[1]),
                    }
                },
            };

        Assert.Equal(expectedPairs, pairs);
    }

    [Fact]
    public void GetPlayersPairs_WhenCalled_WrongResults()
    {
        // Arrange
        Team team1 = new(Guid.NewGuid(),
                         "Team1",
                         players: new[]
                                  {
                                      new Player(Guid.NewGuid(), "Player 1", Gender.Male, 2000),
                                      new Player(Guid.NewGuid(), "Player 2", Gender.Male, 2000),
                                  });
        Team team2 = new(Guid.NewGuid(),
                         "Team2",
                         players: new[]
                                  {
                                      new Player(Guid.NewGuid(), "Player 3", Gender.Male, 2000),
                                      new Player(Guid.NewGuid(), "Player 4", Gender.Male, 2000),
                                  });
        Team team3 = new(Guid.NewGuid(),
                         "Team3",
                         players: new[]
                                  {
                                      new Player(Guid.NewGuid(), "Player 5", Gender.Male, 2000),
                                      new Player(Guid.NewGuid(), "Player 6", Gender.Male, 2000),
                                  });

        Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> pairings = new()
                                                                        {
                                                                            {
                                                                                1,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1, team2),
                                                                                }
                                                                            },
                                                                            {
                                                                                2,
                                                                                new HashSet<GamePair<Team>>
                                                                                {
                                                                                    new(team1,
                                                                                        team3,
                                                                                        GameResult.WhiteWin),
                                                                                }
                                                                            },
                                                                        };


        TeamTournament tournament = new(Guid.NewGuid(),
                                        "Tournament",
                                        DrawSystem.RoundRobin,
                                        new[] { DrawCoefficient.Berger },
                                        4,
                                        DateOnly.FromDateTime(DateTime.UtcNow),
                                        gamePairs: pairings)
                                    {
                                        Teams  = new HashSet<Team>(new[] { team1, team2, team3 }),
                                        Groups = new HashSet<Group>(),
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
                        new(team1.Players.ToArray()[1], team2.Players.ToArray()[1]),
                    }
                },
                {
                    2,
                    new HashSet<GamePair<Player>>
                    {
                        new(team1.Players.ToArray()[0], team3.Players.ToArray()[0]),
                        new(team1.Players.ToArray()[1], team3.Players.ToArray()[1]),
                    }
                },
            };

        Assert.NotEqual(expectedPairs, pairs);
    }
}
