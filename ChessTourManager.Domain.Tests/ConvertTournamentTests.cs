﻿using System.Collections.Immutable;
using ChessTourManager.Domain.Entities;
using ChessTourManager.Domain.Interfaces;
using ChessTourManager.Domain.ValueObjects;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable TooManyArguments
// ReSharper disable TooManyDeclarations
// ReSharper disable ClassTooBig
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable MethodTooLong

namespace ChessTourManager.Domain.Tests;

public sealed class ConvertTournamentTests
{
    #region Arranged

    private readonly Guid     _guid = Guid.NewGuid();
    private readonly Id<Guid> _id;
    private const    string   Name = "Test";

    private readonly DateOnly   _createdAt        = new(2021, 1, 1);
    private const    int        MaxTour           = 1;
    private const    bool       AllowInGroupGames = false;
    private const    DrawSystem System            = DrawSystem.RoundRobin;

    private readonly DateOnly   _createdAt2        = new(2021, 1, 1);
    private const    int        MaxTour2           = 7;
    private const    bool       AllowInGroupGames2 = true;
    private const    DrawSystem System2            = DrawSystem.Swiss;

    private readonly ImmutableArray<Group> _groups = ImmutableArray<Group>.Empty;
    private readonly ImmutableArray<Team>  _teams  = ImmutableArray<Team>.Empty;

    private readonly Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>> _gamePairs = new();

    private readonly Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> _teamPairs = new();

    private readonly ImmutableArray<DrawCoefficient> _coefficients =
        ImmutableArray<DrawCoefficient>.Empty;

    private readonly ImmutableArray<Group> _groups2;

    private readonly ImmutableArray<Team> _teams2;

    private readonly Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>> _gamePairs2;

    private readonly Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> _teamPairs2;

    private readonly ImmutableArray<DrawCoefficient> _coefficients2;

    private readonly ImmutableArray<Guid> _guids;

    private readonly ImmutableArray<string> _groupNames;

    private readonly ImmutableArray<string> _teamNames;

    private readonly ImmutableArray<string> _playerNames;

    private readonly ImmutableArray<Player> _players;

    private readonly SingleTournament _singleTournament;

    private readonly SingleTournament _singleTournament2;

    private readonly SingleTeamTournament _singleTeamTournament;

    private readonly SingleTeamTournament _singleTeamTournament2;

    private readonly TeamTournament _teamTournament;

    private readonly TeamTournament _teamTournament2;

    public ConvertTournamentTests()
    {
        _playerNames = ImmutableArray.Create("Player1",      "Player2",      "Player3",      "Player4");
        _teamNames   = ImmutableArray.Create("Team1",        "Team2",        "Team3",        "Team4");
        _groupNames  = ImmutableArray.Create("Group1",       "Group2",       "Group3",       "Group4");
        _guids       = ImmutableArray.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _players = ImmutableArray.Create(new Player(_guids[0], _playerNames[0], Gender.Male, 2000),
                                         new Player(_guids[1], _playerNames[1], Gender.Male, 2000),
                                         new Player(_guids[2], _playerNames[2], Gender.Male, 2000),
                                         new Player(_guids[3], _playerNames[3], Gender.Male, 2000));

        _groups2 =
            ImmutableArray.Create(new Group(_guids[0], _groupNames[0], _players[..2].ToHashSet()),
                                  new Group(_guids[1], _groupNames[1], _players[2..].ToHashSet()));
        _id = _guid;

        _teams2 =
            ImmutableArray.Create(new Team(_guids[0], _teamNames[0], players: new[] { _players[0] }),
                                  new Team(_guids[1], _teamNames[1], players: new[] { _players[1] }),
                                  new Team(_guids[2], _teamNames[2], players: new[] { _players[2] }),
                                  new Team(_guids[3], _teamNames[3], players: new[] { _players[3] }));

        _coefficients2 = ImmutableArray.Create(DrawCoefficient.Buchholz,
                                               DrawCoefficient.TotalBuchholz);

        _gamePairs2 =
            new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>
                (
                 new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>
                 {
                     {
                         new TourNumber(1), new HashSet<GamePair<Player>>
                                            {
                                                new(_players[0], _players[1]),
                                                new(_players[2], _players[3]),
                                            }
                     },
                     {
                         new TourNumber(2), new HashSet<GamePair<Player>>
                                            {
                                                new(_players[0], _players[2]),
                                                new(_players[1], _players[3]),
                                            }
                     },
                 });

        _teamPairs2 =
            new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>
                (
                 new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>
                 {
                     {
                         new TourNumber(1), new HashSet<GamePair<Team>>
                                            {
                                                new(_teams2[0], _teams2[1]),
                                                new(_teams2[2], _teams2[3]),
                                            }
                     },
                     {
                         new TourNumber(2), new HashSet<GamePair<Team>>
                                            {
                                                new(_teams2[0], _teams2[2]),
                                                new(_teams2[1], _teams2[3]),
                                            }
                     },
                 });

        _singleTournament = new SingleTournament(_id,
                                                 Name,
                                                 System,
                                                 _coefficients,
                                                 MaxTour,
                                                 _createdAt,
                                                 AllowInGroupGames,
                                                 _gamePairs)
                            {
                                Groups = new HashSet<Group>(_groups),
                            };

        _singleTournament2 = new SingleTournament(_id,
                                                  Name,
                                                  System2,
                                                  _coefficients2,
                                                  MaxTour2,
                                                  _createdAt2,
                                                  AllowInGroupGames2,
                                                  _gamePairs2)
                             {
                                 Groups = new HashSet<Group>(_groups2),
                             };
        _singleTeamTournament = new SingleTeamTournament(_id,
                                                         Name,
                                                         System,
                                                         _coefficients,
                                                         MaxTour,
                                                         _createdAt,
                                                         gamePairs: _gamePairs,
                                                         teams: new HashSet<Team>(_teams))
                                {
                                    Groups = new HashSet<Group>(_groups),
                                };
        _singleTeamTournament2 = new SingleTeamTournament(_id,
                                                          Name,
                                                          System2,
                                                          _coefficients2,
                                                          MaxTour2,
                                                          _createdAt2,
                                                          AllowInGroupGames2,
                                                          teams: new HashSet<Team>(_teams2),
                                                          gamePairs: _gamePairs2)
                                 {
                                     Groups = new HashSet<Group>(_groups2),
                                 };
        _teamTournament = new TeamTournament(_id,
                                             Name,
                                             System,
                                             _coefficients,
                                             MaxTour,
                                             _createdAt,
                                             teams: _teams.ToHashSet(),
                                             gamePairs: _teamPairs)

                          {
                              Groups = new HashSet<Group>(_groups),
                          };
        _teamTournament2 = new TeamTournament(_id,
                                              Name,
                                              System2,
                                              _coefficients2,
                                              MaxTour2,
                                              _createdAt2,
                                              AllowInGroupGames2,
                                              teams: _teams2.ToHashSet(),
                                              gamePairs: _teamPairs2)
                           {
                               Groups = new HashSet<Group>(_groups2),
                           };
    }

    #endregion Arranged

    #region Convert from Single

    [Fact]
    public void ConvertFromSingleToSingle1()
    {
        // Arrange
        SingleTournament tournament = _singleTournament;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertDefault<SingleTournament, Player>(result, TournamentKind.Single);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleToSingle2()
    {
        // Arrange
        SingleTournament tournament = _singleTournament2;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertDefault2<SingleTournament, Player>(result, TournamentKind.Single);
        AssertPlayersGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(result.GamePairs));
    }

    [Fact]
    public void ConvertFromSingleToSingleTeam1()
    {
        // Arrange
        SingleTournament tournament = _singleTournament;

        // Act
        SingleTeamTournament result = tournament.ConvertToSingleTeamTournament();

        // Assert
        AssertDefault<SingleTeamTournament, Player>(result, TournamentKind.SingleTeam);
        AssertTeam(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleToSingleTeam2()
    {
        // Arrange
        SingleTournament tournament = _singleTournament2;

        // Act
        SingleTeamTournament result = tournament.ConvertToSingleTeamTournament();
        Assert.True(result.TryAddTeam(_teams2[0]));
        Assert.True(result.TryAddTeam(_teams2[1]));
        Assert.True(result.TryAddTeam(_teams2[2]));
        Assert.True(result.TryAddTeam(_teams2[3]));

        // Assert
        AssertDefault2<SingleTeamTournament, Player>(result, TournamentKind.SingleTeam);
        AssertTeam2(result);
        AssertPlayersGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(result.GamePairs));
    }

    [Fact]
    public void ConvertFromSingleToTeam1()
    {
        // Arrange
        SingleTournament tournament = _singleTournament;

        // Act
        TeamTournament result = tournament.ConvertToTeamTournament();

        // Assert
        AssertDefault<TeamTournament, Team>(result, TournamentKind.Team);
        AssertTeam(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleToTeam2()
    {
        // Arrange
        SingleTournament tournament = _singleTournament2;

        // Act
        TeamTournament result = tournament.ConvertToTeamTournament();
        Assert.True(result.TryAddTeam(_teams2[0]));
        Assert.True(result.TryAddTeam(_teams2[1]));
        Assert.True(result.TryAddTeam(_teams2[2]));
        Assert.True(result.TryAddTeam(_teams2[3]));

        // Assert
        AssertDefault2<TeamTournament, Team>(result, TournamentKind.Team);
        AssertTeam2(result);
        Assert.Empty(result.GamePairs);
    }

    #endregion Convert from Single

    #region Convert from Single-Team

    [Fact]
    public void ConvertFromSingleTeamToSingle1()
    {
        // Arrange
        SingleTeamTournament tournament = _singleTeamTournament;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertDefault<SingleTournament, Player>(result, TournamentKind.Single);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleTeamToSingle2()
    {
        // Arrange
        SingleTeamTournament tournament = _singleTeamTournament2;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertDefault2<SingleTournament, Player>(result, TournamentKind.Single);
        AssertPlayersGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(result.GamePairs));
    }

    [Fact]
    public void ConvertFromSingleTeamToTeam1()
    {
        // Arrange
        SingleTeamTournament tournament = _singleTeamTournament;

        // Act
        TeamTournament result = tournament.ConvertToTeamTournament();

        // Assert
        AssertDefault<TeamTournament, Team>(result, TournamentKind.Team);
        AssertTeam(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleTeamToTeam2()
    {
        // Arrange
        SingleTeamTournament tournament = _singleTeamTournament2;

        // Act
        TeamTournament result = tournament.ConvertToTeamTournament();

        // Assert
        AssertDefault2<TeamTournament, Team>(result, TournamentKind.Team);
        AssertTeam2(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleTeamToSingleTeam1()
    {
        // Arrange
        SingleTeamTournament tournament = _singleTeamTournament;

        // Act
        SingleTeamTournament result = tournament.ConvertToSingleTeamTournament();

        // Assert
        AssertDefault<SingleTeamTournament, Player>(result, TournamentKind.SingleTeam);
        AssertTeam(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromSingleTeamToSingleTeam2()
    {
        // Arrange
        SingleTeamTournament tournament = _singleTeamTournament2;

        // Act
        SingleTeamTournament result = tournament.ConvertToSingleTeamTournament();

        // Assert
        AssertDefault2<SingleTeamTournament, Player>(result, TournamentKind.SingleTeam);
        AssertTeam2(result);
        AssertPlayersGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(result.GamePairs));
    }

    #endregion Convert from Single-Team

    #region Convert from Team

    [Fact]
    public void ConvertFromTeamToSingle1()
    {
        // Arrange
        TeamTournament tournament = _teamTournament;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertDefault<SingleTournament, Player>(result, TournamentKind.Single);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromTeamToSingle2()
    {
        // Arrange
        TeamTournament tournament = _teamTournament2;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        AssertDefault2<SingleTournament, Player>(result, TournamentKind.Single);
        AssertPlayersGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(result.GamePairs));
    }

    [Fact]
    public void ConvertFromTeamToSingleTeam1()
    {
        // Arrange
        TeamTournament tournament = _teamTournament;

        // Act
        SingleTeamTournament result = tournament.ConvertToSingleTeamTournament();

        // Assert
        AssertDefault<SingleTeamTournament, Player>(result, TournamentKind.SingleTeam);
        AssertTeam(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromTeamToSingleTeam2()
    {
        // Arrange
        TeamTournament tournament = _teamTournament2;

        // Act
        SingleTeamTournament result = tournament.ConvertToSingleTeamTournament();

        // Assert
        AssertDefault2<SingleTeamTournament, Player>(result, TournamentKind.SingleTeam);
        AssertTeam2(result);
        AssertPlayersGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>(result.GamePairs));
    }

    [Fact]
    public void ConvertFromTeamToTeam1()
    {
        // Arrange
        TeamTournament tournament = _teamTournament;

        // Act
        TeamTournament result = tournament.ConvertToTeamTournament();

        // Assert
        AssertDefault<TeamTournament, Team>(result, TournamentKind.Team);
        AssertTeam(result);
        Assert.Empty(result.GamePairs);
    }

    [Fact]
    public void ConvertFromTeamToTeam2()
    {
        // Arrange
        TeamTournament tournament = _teamTournament2;

        // Act
        TeamTournament result = tournament.ConvertToTeamTournament();

        // Assert
        AssertDefault2<TeamTournament, Team>(result, TournamentKind.Team);
        AssertTeam2(result);
        AssertTeamsGamePairs(new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>(result.GamePairs));
    }

    #endregion Convert from Team

    #region Assertions

    private void AssertDefault<TTournament, TPlayer>(TTournament result, TournamentKind kind)
        where TTournament : DrawableTournament<TPlayer> where TPlayer : Participant<TPlayer>
    {
        Assert.Equal(new Id<Guid>(_guid),                                           result.Id);
        Assert.Equal(new Name("Test"),                                              result.Name);
        Assert.Equal(DrawSystem.RoundRobin,                                         result.System);
        Assert.Equal(new List<DrawCoefficient>(),                                   result.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1),                                      result.CreatedAt);
        Assert.Equal(kind,                                                          result.Kind);
        Assert.Equal(new TourNumber(1),                                             result.MaxTour);
        Assert.Equal(TourNumber.BeforeStart(),                                      result.CurrentTour);
        Assert.Equal(new List<Group>(),                                             result.Groups);
        Assert.Equal(false,                                                         result.AllowMixGroupGames);
        Assert.Equal(new Dictionary<TourNumber, IReadOnlySet<GamePair<TPlayer>>>(), result.GamePairs);
    }

    private void AssertDefault2<TTournament, TPlayer>(TTournament result, TournamentKind kind)
        where TTournament : DrawableTournament<TPlayer> where TPlayer : Participant<TPlayer>
    {
        Assert.Equal(new Id<Guid>(_guid), result.Id);
        Assert.Equal(new Name("Test"),    result.Name);
        Assert.Equal(DrawSystem.Swiss,    result.System);
        Assert.Equal(new List<DrawCoefficient>
                     { DrawCoefficient.Buchholz, DrawCoefficient.TotalBuchholz },
                     result.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.Equal(kind,                     result.Kind);
        Assert.Equal(new TourNumber(7),        result.MaxTour);
        Assert.Equal(TourNumber.BeforeStart(), result.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(_guids[0], _groupNames[0], _players[..2].ToHashSet()),
                         new(_guids[1], _groupNames[1], _players[2..].ToHashSet()),
                     },
                     result.Groups);
        Assert.Equal(true, result.AllowMixGroupGames);
    }

    private void AssertPlayersGamePairs(Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>> result)
    {
        Assert.Equal(GetPlayersPairings(), result);

        Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>> GetPlayersPairings()
        {

            return new Dictionary<TourNumber, IReadOnlySet<GamePair<Player>>>
                   {
                       {
                           new TourNumber(1), new HashSet<GamePair<Player>>
                                              {
                                                  new(new Player(_guids[0],
                                                                 _playerNames[0],
                                                                 Gender.Male,
                                                                 2000),
                                                      new Player(_guids[1],
                                                                 _playerNames[1],
                                                                 Gender.Male,
                                                                 2000)),
                                                  new(new Player(_guids[2],
                                                                 _playerNames[2],
                                                                 Gender.Male,
                                                                 2000),
                                                      new Player(_guids[3],
                                                                 _playerNames[3],
                                                                 Gender.Male,
                                                                 2000)),
                                              }
                       },
                       {
                           new TourNumber(2), new HashSet<GamePair<Player>>
                                              {
                                                  new(new Player(_guids[0],
                                                                 _playerNames[0],
                                                                 Gender.Male,
                                                                 2000),
                                                      new Player(_guids[2],
                                                                 _playerNames[2],
                                                                 Gender.Male,
                                                                 2000)),
                                                  new(new Player(_guids[1],
                                                                 _playerNames[1],
                                                                 Gender.Male,
                                                                 2000),
                                                      new Player(_guids[3],
                                                                 _playerNames[3],
                                                                 Gender.Male,
                                                                 2000)),
                                              }
                       },
                   };
        }
    }


    private void AssertTeamsGamePairs(Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> result)
    {
        Assert.Equal(GetTeamsPairings(), result);

        Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>> GetTeamsPairings()
        {

            return new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>
                (
                 new Dictionary<TourNumber, IReadOnlySet<GamePair<Team>>>
                 {
                     {
                         new TourNumber(1), new HashSet<GamePair<Team>>
                                            {
                                                new(_teams2[0], _teams2[1]),
                                                new(_teams2[2], _teams2[3]),
                                            }
                     },
                     {
                         new TourNumber(2), new HashSet<GamePair<Team>>
                                            {
                                                new(_teams2[0], _teams2[2]),
                                                new(_teams2[1], _teams2[3]),
                                            }
                     },
                 });
        }
    }

    private static void AssertTeam(ITeamTournament result)
    {
        Assert.Equal(new HashSet<Team>(), result.Teams);
    }

    private void AssertTeam2(ITeamTournament result)
    {
        Assert.Equal(new HashSet<Team>
                     {
                         new(_guids[0], _teamNames[0], players: new[] { _players[0] }),
                         new(_guids[1], _teamNames[1], players: new[] { _players[1] }),
                         new(_guids[2], _teamNames[2], players: new[] { _players[2] }),
                         new(_guids[3], _teamNames[3], players: new[] { _players[3] }),
                     },
                     result.Teams);
    }

    #endregion Assertions
}
