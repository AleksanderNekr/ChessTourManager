using System.Collections.Immutable;
using System.Collections.ObjectModel;
using ChessTourManager.Domain.Entities;
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

    private readonly DateOnly                  _createdAt        = new(2021, 1, 1);
    private const    int                       MaxTour           = 1;
    private const    int                       CurrentTour       = 1;
    private const    bool                      AllowInGroupGames = false;
    private const    TournamentBase.DrawSystem DrawSystem        = TournamentBase.DrawSystem.RoundRobin;

    private readonly DateOnly                  _createdAt2        = new(2021, 1, 1);
    private const    int                       MaxTour2           = 7;
    private const    int                       CurrentTour2       = 2;
    private const    bool                      AllowInGroupGames2 = true;
    private const    TournamentBase.DrawSystem DrawSystem2        = TournamentBase.DrawSystem.Swiss;

    private readonly ImmutableArray<Group> _groups = ImmutableArray<Group>.Empty;
    private readonly ImmutableArray<Team>  _teams  = ImmutableArray<Team>.Empty;

    private readonly Dictionary<TourNumber, IReadOnlySet<GamePair>> _gamePairs = new();

    private readonly ImmutableArray<TournamentBase.DrawCoefficient> _coefficients =
        ImmutableArray<TournamentBase.DrawCoefficient>.Empty;

    private readonly ImmutableArray<Group> _groups2;

    private readonly ImmutableArray<Team> _teams2;

    private readonly Dictionary<TourNumber, IReadOnlySet<GamePair>> _gamePairs2;

    private readonly ImmutableArray<TournamentBase.DrawCoefficient> _coefficients2;

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
        this._playerNames = ImmutableArray.Create("Player1",      "Player2",      "Player3",      "Player4");
        this._teamNames   = ImmutableArray.Create("Team1",        "Team2",        "Team3",        "Team4");
        this._groupNames  = ImmutableArray.Create("Group1",       "Group2",       "Group3",       "Group4");
        this._guids       = ImmutableArray.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        this._players = ImmutableArray.Create(new Player(this._guids[0], this._playerNames[0]),
                                              new Player(this._guids[1], this._playerNames[1]),
                                              new Player(this._guids[2], this._playerNames[2]),
                                              new Player(this._guids[3], this._playerNames[3]));

        this._groups2 = ImmutableArray.Create(new Group(this._guids[0], this._groupNames[0], this._players[..2]),
                                              new Group(this._guids[1], this._groupNames[1], this._players[2..]));
        this._id = this._guid;

        this._teams2 = ImmutableArray.Create(new Team(this._guids[0], this._teamNames[0], this._players[..2]),
                                             new Team(this._guids[1], this._teamNames[1], this._players[2..]));

        this._coefficients2 = ImmutableArray.Create(TournamentBase.DrawCoefficient.Buchholz,
                                                    TournamentBase.DrawCoefficient.TotalBuchholz);

        this._gamePairs2 =
            new Dictionary<TourNumber, IReadOnlySet<GamePair>>
                (
                 new Dictionary<TourNumber, IReadOnlySet<GamePair>>
                 {
                     {
                         new TourNumber(1), new HashSet<GamePair>
                                            {
                                                new(this._players[0], this._players[1]),
                                                new(this._players[2], this._players[3])
                                            }
                     },
                     {
                         new TourNumber(2), new HashSet<GamePair>
                                            {
                                                new(this._players[0], this._players[2]),
                                                new(this._players[1], this._players[3])
                                            }
                     },
                 });

        this._singleTournament = TournamentBase.CreateSingleTournament(this._id,
                                                                       Name,
                                                                       DrawSystem,
                                                                       this._coefficients,
                                                                       MaxTour,
                                                                       CurrentTour,
                                                                       new List<Group>(this._groups),
                                                                       this._createdAt,
                                                                       AllowInGroupGames,
                                                                       new Dictionary<TourNumber,
                                                                           IReadOnlySet<GamePair>>(this._gamePairs));
        this._singleTournament2 = TournamentBase.CreateSingleTournament(this._id,
                                                                        Name,
                                                                        DrawSystem2,
                                                                        this._coefficients2,
                                                                        MaxTour2,
                                                                        CurrentTour2,
                                                                        new List<Group>(this._groups2),
                                                                        this._createdAt2,
                                                                        AllowInGroupGames2,
                                                                        new Dictionary<TourNumber,
                                                                            IReadOnlySet<GamePair>>(this._gamePairs2));
        this._singleTeamTournament = TournamentBase.CreateTeamTournament<SingleTeamTournament>(this._id,
            Name,
            DrawSystem,
            this._coefficients,
            MaxTour,
            CurrentTour,
            new List<Group>(this._groups),
            this._createdAt,
            new List<Team>(this._teams),
            AllowInGroupGames,
            new Dictionary<TourNumber,
                IReadOnlySet<GamePair>>(this._gamePairs));
        this._singleTeamTournament2 = TournamentBase.CreateTeamTournament<SingleTeamTournament>(this._id,
            Name,
            DrawSystem2,
            this._coefficients2,
            MaxTour2,
            CurrentTour2,
            new List<Group>(this._groups2),
            this._createdAt2,
            new List<Team>(this._teams2),
            AllowInGroupGames2,
            new Dictionary<TourNumber,
                IReadOnlySet<GamePair>>(this._gamePairs2));
        this._teamTournament = TournamentBase.CreateTeamTournament<TeamTournament>(this._id,
            Name,
            DrawSystem,
            this._coefficients,
            MaxTour,
            CurrentTour,
            new List<Group>(this._groups),
            this._createdAt,
            new List<Team>(this._teams),
            AllowInGroupGames,
            new Dictionary<TourNumber, IReadOnlySet<GamePair>>(this._gamePairs));
        this._teamTournament2 = TournamentBase.CreateTeamTournament<TeamTournament>(this._id,
            Name,
            DrawSystem2,
            this._coefficients2,
            MaxTour2,
            CurrentTour2,
            new List<Group>(this._groups2),
            this._createdAt2,
            new List<Team>(this._teams2),
            AllowInGroupGames2,
            new Dictionary<TourNumber, IReadOnlySet<GamePair>>(this._gamePairs2));
    }

    #endregion Arranged


    #region Convert from Single

    [Fact]
    public void ConvertFromSingleToSingle1()
    {
        // Arrange
        SingleTournament tournament = this._singleTournament;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.Single);
    }

    [Fact]
    public void ConvertFromSingleToSingle2()
    {
        // Arrange
        SingleTournament tournament = this._singleTournament2;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.Single);
    }

    [Fact]
    public void ConvertFromSingleToSingleTeam1()
    {
        // Arrange
        SingleTournament tournament = this._singleTournament;

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.SingleTeam);
        AssertTeam(result);
    }

    [Fact]
    public void ConvertFromSingleToSingleTeam2()
    {
        // Arrange
        SingleTournament tournament = this._singleTournament2;

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();
        result.Teams.Add(this._teams2[0]);
        result.Teams.Add(this._teams2[1]);

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.SingleTeam);
        this.AssertTeam2(result);
    }

    [Fact]
    public void ConvertFromSingleToTeam1()
    {
        // Arrange
        SingleTournament tournament = this._singleTournament;

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.Team);
        AssertTeam(result);
    }

    [Fact]
    public void ConvertFromSingleToTeam2()
    {
        // Arrange
        SingleTournament tournament = this._singleTournament2;

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();
        result.Teams.Add(this._teams2[0]);
        result.Teams.Add(this._teams2[1]);

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.Team);
        this.AssertTeam2(result);
    }

    #endregion Convert from Single

    #region Convert from Single-Team

    [Fact]
    public void ConvertFromSingleTeamToSingle1()
    {
        // Arrange
        SingleTeamTournament tournament = this._singleTeamTournament;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.Single);
    }

    [Fact]
    public void ConvertFromSingleTeamToSingle2()
    {
        // Arrange
        SingleTeamTournament tournament = this._singleTeamTournament2;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.Single);
    }

    [Fact]
    public void ConvertFromSingleTeamToTeam1()
    {
        // Arrange
        SingleTeamTournament tournament = this._singleTeamTournament;

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.Team);
        AssertTeam(result);
    }

    [Fact]
    public void ConvertFromSingleTeamToTeam2()
    {
        // Arrange
        SingleTeamTournament tournament = this._singleTeamTournament2;

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.Team);
        this.AssertTeam2(result);
    }

    [Fact]
    public void ConvertFromSingleTeamToSingleTeam1()
    {
        // Arrange
        SingleTeamTournament tournament = this._singleTeamTournament;

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.SingleTeam);
        AssertTeam(result);
    }

    [Fact]
    public void ConvertFromSingleTeamToSingleTeam2()
    {
        // Arrange
        SingleTeamTournament tournament = this._singleTeamTournament2;

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.SingleTeam);
        this.AssertTeam2(result);
    }

    #endregion Convert from Single-Team

    #region Convert from Team

    [Fact]
    public void ConvertFromTeamToSingle1()
    {
        // Arrange
        TeamTournament tournament = this._teamTournament;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.Single);
    }

    [Fact]
    public void ConvertFromTeamToSingle2()
    {
        // Arrange
        TeamTournament tournament = this._teamTournament2;

        // Act
        SingleTournament result = tournament.ConvertToSingleTournament();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.Single);
    }

    [Fact]
    public void ConvertFromTeamToSingleTeam1()
    {
        // Arrange
        TeamTournament tournament = this._teamTournament;

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.SingleTeam);
        AssertTeam(result);
    }

    [Fact]
    public void ConvertFromTeamToSingleTeam2()
    {
        // Arrange
        TeamTournament tournament = this._teamTournament2;

        // Act
        var result = tournament.ConvertToTeamTournament<SingleTeamTournament>();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.SingleTeam);
        this.AssertTeam2(result);
    }

    [Fact]
    public void ConvertFromTeamToTeam1()
    {
        // Arrange
        TeamTournament tournament = this._teamTournament;

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        this.AssertDefault(result, TournamentBase.TournamentKind.Team);
        AssertTeam(result);
    }

    [Fact]
    public void ConvertFromTeamToTeam2()
    {
        // Arrange
        TeamTournament tournament = this._teamTournament2;

        // Act
        var result = tournament.ConvertToTeamTournament<TeamTournament>();

        // Assert
        this.AssertDefault2(result, TournamentBase.TournamentKind.Team);
        this.AssertTeam2(result);
    }

    #endregion Convert from Team

    #region Assertions

    private void AssertDefault(TournamentBase result, TournamentBase.TournamentKind kind)
    {
        Assert.Equal(new Id<Guid>(this._guid),                        result.Id);
        Assert.Equal(new Name("Test"),                                result.Name);
        Assert.Equal(TournamentBase.DrawSystem.RoundRobin,            result.System);
        Assert.Equal(new List<TournamentBase.DrawCoefficient>(),      result.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1),                        result.CreatedAt);
        Assert.Equal(kind,                                            result.Kind);
        Assert.Equal(new TourNumber(1),                               result.MaxTour);
        Assert.Equal(new TourNumber(1),                               result.CurrentTour);
        Assert.Equal(new List<Group>(),                               result.Groups);
        Assert.Equal(false,                                           result.AllowInGroupGames);
        Assert.Equal(new Dictionary<TourNumber, IReadOnlySet<GamePair>>(), result.GamePairs);
    }

    private void AssertDefault2(TournamentBase result, TournamentBase.TournamentKind kind)
    {
        Assert.Equal(new Id<Guid>(this._guid),        result.Id);
        Assert.Equal(new Name("Test"),                result.Name);
        Assert.Equal(TournamentBase.DrawSystem.Swiss, result.System);
        Assert.Equal(new List<TournamentBase.DrawCoefficient>
                     { TournamentBase.DrawCoefficient.Buchholz, TournamentBase.DrawCoefficient.TotalBuchholz },
                     result.Coefficients);
        Assert.Equal(new DateOnly(2021, 1, 1), result.CreatedAt);
        Assert.Equal(kind,                     result.Kind);
        Assert.Equal(new TourNumber(7),        result.MaxTour);
        Assert.Equal(new TourNumber(2),        result.CurrentTour);
        Assert.Equal(new List<Group>
                     {
                         new(this._guids[0], this._groupNames[0], this._players[..2]),
                         new(this._guids[1], this._groupNames[1], this._players[2..]),
                     },
                     result.Groups);
        Assert.Equal(true, result.AllowInGroupGames);
        Assert.Equal(new Dictionary<TourNumber, IReadOnlySet<GamePair>>
                     {
                         {
                             new TourNumber(1), new HashSet<GamePair>
                                                {
                                                    new(new Player(this._guids[0], this._playerNames[0]),
                                                        new Player(this._guids[1], this._playerNames[1])),
                                                    new(new Player(this._guids[2], this._playerNames[2]),
                                                        new Player(this._guids[3], this._playerNames[3])),
                                                }
                         },
                         {
                             new TourNumber(2), new HashSet<GamePair>
                                                {
                                                    new(new Player(this._guids[0], this._playerNames[0]),
                                                        new Player(this._guids[2], this._playerNames[2])),
                                                    new(new Player(this._guids[1], this._playerNames[1]),
                                                        new Player(this._guids[3], this._playerNames[3])),
                                                }
                         },
                     },
                     result.GamePairs);
    }

    private static void AssertTeam(ITeamTournament result)
    {
        Assert.Equal(new List<Team>(), result.Teams);
    }

    private void AssertTeam2(ITeamTournament result)
    {
        Assert.Equal(new List<Team>
                     {
                         new(this._guids[0], this._teamNames[0], this._players[..2]),
                         new(this._guids[1], this._teamNames[1], this._players[2..]),
                     },
                     result.Teams);
    }

    #endregion Assertions
}
