using System.Collections.Immutable;
using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

// ReSharper disable TooManyDependencies
// ReSharper disable TooManyArguments

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase : INameable
{
    public enum DrawCoefficient
    {
        Berger,
        SimpleBerger,
        Buchholz,
        TotalBuchholz,
    }

    public enum DrawSystem
    {
        RoundRobin,
        Swiss,
    }

    public enum TournamentKind
    {
        Single,
        Team,
        SingleTeam,
    }

    private protected TournamentBase(Id<Guid>                                                id,
                                     Name                                                    name,
                                     DrawSystem                                              drawSystem,
                                     IReadOnlyCollection<DrawCoefficient>                    coefficients,
                                     TourNumber                                              maxTour,
                                     DateOnly                                                createdAt,
                                     TourNumber                                              currentTour,
                                     ICollection<Group>                                      groups,
                                     bool                                                    allowInGroupGames,
                                     IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair>> gamePairs)
    {
        this.Id                = id;
        this.Name              = name;
        this.CreatedAt         = createdAt;
        this.Groups            = groups;
        this.AllowInGroupGames = allowInGroupGames;
        this.GamePairs         = gamePairs;
        this.SetDrawingProperties(drawSystem, coefficients);
        this.SetTours(maxTour, currentTour);
    }

    public Id<Guid> Id { get; }

    public Name Name { get; }

    public DateOnly CreatedAt { get; }

    public DrawSystem System { get; private set; }

    public IReadOnlyCollection<DrawCoefficient> Coefficients { get; private set; } = default!;

    public TournamentKind Kind { get; private protected init; }

    public TourNumber MaxTour { get; private set; }

    public TourNumber CurrentTour { get; private set; }

    public ICollection<Group> Groups { get; }

    public bool AllowInGroupGames { get; }

    public IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair>> GamePairs { get; }

    public IReadOnlyCollection<Player> Players
    {
        get => this.Groups.SelectMany(static g => g.Players).ToImmutableList();
    }

    public static SingleTournament CreateSingleTournament(Id<Guid>                                                id,
                                                          Name                                                    name,
                                                          DrawSystem                                              drawSystem,
                                                          IReadOnlyCollection<DrawCoefficient>                    coefficients,
                                                          TourNumber                                              maxTour,
                                                          TourNumber                                              currentTour,
                                                          ICollection<Group>                                      groups,
                                                          DateOnly                                                createdAt,
                                                          bool                                                    allowInGroupGames,
                                                          IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair>> gamePairs)
    {
        return new SingleTournament(id,
                                    name,
                                    drawSystem,
                                    coefficients,
                                    maxTour,
                                    createdAt,
                                    currentTour,
                                    groups,
                                    allowInGroupGames,
                                    gamePairs);

    }

    public static TTournament CreateTeamTournament<TTournament>(Id<Guid>                                                id,
                                                                Name                                                    name,
                                                                DrawSystem                                              drawSystem,
                                                                IReadOnlyCollection<DrawCoefficient>                    coefficients,
                                                                TourNumber                                              maxTour,
                                                                TourNumber                                              currentTour,
                                                                ICollection<Group>                                      groups,
                                                                DateOnly                                                createdAt,
                                                                ICollection<Team>                                       teams,
                                                                bool                                                    allowInGroupGames,
                                                                IReadOnlyDictionary<TourNumber, IReadOnlySet<GamePair>> gamePairs)
        where TTournament : ITeamTournament
    {
        ITeamTournament tournament;
        if (typeof(TTournament) == typeof(SingleTeamTournament))
        {
            tournament = new SingleTeamTournament(id,
                                                  name,
                                                  drawSystem,
                                                  coefficients,
                                                  maxTour,
                                                  createdAt,
                                                  currentTour,
                                                  groups,
                                                  teams,
                                                  allowInGroupGames,
                                                  gamePairs);
        }
        else if (typeof(TTournament) == typeof(TeamTournament))
        {
            tournament = new TeamTournament(id,
                                            name,
                                            drawSystem,
                                            coefficients,
                                            maxTour,
                                            createdAt,
                                            currentTour,
                                            groups,
                                            teams,
                                            allowInGroupGames,
                                            gamePairs);
        }
        else
        {
            throw new DomainOutOfRangeException(nameof(TTournament), typeof(TTournament));
        }

        return (TTournament)tournament;
    }

    private static IEnumerable<DrawCoefficient> GetPossibleCoefficients(DrawSystem drawSystem)
    {
        return drawSystem switch
               {
                   DrawSystem.RoundRobin => new List<DrawCoefficient>
                                            {
                                                DrawCoefficient.Berger,
                                                DrawCoefficient.SimpleBerger,
                                            },
                   DrawSystem.Swiss => new List<DrawCoefficient>
                                       {
                                           DrawCoefficient.Buchholz,
                                           DrawCoefficient.TotalBuchholz,
                                       },
                   _ => throw new DomainOutOfRangeException(nameof(drawSystem), drawSystem),
               };
    }

    public SingleTournament ConvertToSingleTournament()
    {
        return CreateSingleTournament(this.Id,
                                      this.Name,
                                      this.System,
                                      this.Coefficients,
                                      this.MaxTour,
                                      this.CurrentTour,
                                      this.Groups,
                                      this.CreatedAt,
                                      this.AllowInGroupGames,
                                      this.GamePairs);
    }

    public TTournament ConvertToTeamTournament<TTournament>() where TTournament : ITeamTournament
    {
        return CreateTeamTournament<TTournament>(this.Id,
                                                 this.Name,
                                                 this.System,
                                                 this.Coefficients,
                                                 this.MaxTour,
                                                 this.CurrentTour,
                                                 createdAt: this.CreatedAt,
                                                 groups: this.Groups,
                                                 teams: (this as ITeamTournament)?.Teams
                                                     ?? new List<Team>(),
                                                 allowInGroupGames: this.AllowInGroupGames,
                                                 gamePairs: this.GamePairs);
    }

    private void SetTours(TourNumber maxTour, TourNumber currentTour)
    {
        if (maxTour < currentTour)
        {
            throw new DomainException("Max tour must be greater or equal to current tour");
        }

        this.MaxTour     = maxTour;
        this.CurrentTour = currentTour;
    }

    private void SetDrawingProperties(DrawSystem drawSystem, IReadOnlyCollection<DrawCoefficient> coefficients)
    {
        this.System = drawSystem;
        this.UpdateCoefficients(coefficients);
    }

    public void UpdateCoefficients(IReadOnlyCollection<DrawCoefficient> coefficients)
    {
        List<DrawCoefficient> wrongCoefficients = coefficients.Except(GetPossibleCoefficients(this.System)).ToList();
        if (wrongCoefficients.Any())
        {
            throw new DomainException($"Wrong coefficients for {this.System} draw system: {
                string.Join(", ", wrongCoefficients)}");
        }

        this.Coefficients = coefficients;
    }

    public DrawResult DrawNewTour()
    {
        return this.System switch
               {
                   DrawSystem.RoundRobin => this.DrawRoundRobin(),
                   DrawSystem.Swiss      => this.DrawSwiss(),
                   _ => throw new DomainOutOfRangeException(nameof(this.System),
                                                            this.System,
                                                            "Cannot draw – unknown system"),
               };
    }

    private protected abstract DrawResult DrawSwiss();

    private protected abstract DrawResult DrawRoundRobin();
}
