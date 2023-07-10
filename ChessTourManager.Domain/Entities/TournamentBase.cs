using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;
// ReSharper disable TooManyDependencies
// ReSharper disable TooManyArguments

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase
{
    private protected TournamentBase(Id<Guid>                         id,
                                     Name                             name,
                                     DrawSystem                       drawSystem,
                                     IReadOnlyCollection<Coefficient> coefficients,
                                     TourNumber                       maxTour,
                                     DateOnly                         createdAt,
                                     TourNumber                       currentTour,
                                     List<Group>                      groups)
    {
        this.Id        = id;
        this.Name      = name;
        this.CreatedAt = createdAt;
        this.Groups    = groups;
        this.SetDrawingProperties(drawSystem, coefficients);
        this.SetTours(maxTour, currentTour);
    }

    public Id<Guid> Id { get; }

    public Name Name { get; }

    public DateOnly CreatedAt { get; }

    public DrawSystem DrawSystem { get; private set; }

    public IReadOnlyCollection<Coefficient> Coefficients { get; private set; } = default!;

    public Kind Kind { get; private protected init; }

    public TourNumber MaxTour { get; private set; }

    public TourNumber CurrentTour { get; private set; }

    public List<Group> Groups { get; }

    public IEnumerable<Player> Players
    {
        get => this.Groups.SelectMany(static g => g.Players);
    }

    public static SingleTournament CreateSingleTournament(Id<Guid>                         id,
                                                Name                             name,
                                                DrawSystem                       drawSystem,
                                                IReadOnlyCollection<Coefficient> coefficients,
                                                TourNumber                       maxTour,
                                                TourNumber                       currentTour,
                                                List<Group>                      groups,
                                                DateOnly                         createdAt)
    {
        return new SingleTournament(id, name, drawSystem, coefficients,
                                    maxTour, createdAt, currentTour, groups);

    }

    public static TTournament CreateTeamTournament<TTournament>(Id<Guid>                         id,
                                                      Name                             name,
                                                      DrawSystem                       drawSystem,
                                                      IReadOnlyCollection<Coefficient> coefficients,
                                                      TourNumber                       maxTour,
                                                      TourNumber                       currentTour,
                                                      List<Group>                      groups,
                                                      DateOnly                         createdAt,
                                                      List<Team>                       teams)
        where TTournament : ITeamTournament
    {
        ITeamTournament tournament;
        if (typeof(TTournament) == typeof(SingleTeamTournament))
        {
            tournament = new SingleTeamTournament(id, name, drawSystem, coefficients,
                                                  maxTour, createdAt, currentTour, groups, teams);
        }
        else if (typeof(TTournament) == typeof(TeamTournament))
        {
            tournament = new TeamTournament(id, name, drawSystem, coefficients,
                                            maxTour, createdAt, currentTour, groups, teams);
        }
        else
        {
            throw new DomainOutOfRangeException(nameof(TTournament), typeof(TTournament));
        }

        return (TTournament)tournament;
    }

    private static IEnumerable<Coefficient> GetPossibleCoefficients(DrawSystem drawSystem)
    {
        return drawSystem switch
               {
                   DrawSystem.RoundRobin => new List<Coefficient> { Coefficient.Berger, Coefficient.SimpleBerger },
                   DrawSystem.Swiss      => new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                   _                     => throw new DomainOutOfRangeException(nameof(drawSystem), drawSystem),
               };
    }

    public SingleTournament ConvertToSingleTournament()
    {
        return CreateSingleTournament(id: this.Id,
                            name: this.Name,
                            drawSystem: this.DrawSystem,
                            coefficients: this.Coefficients,
                            maxTour: this.MaxTour,
                            currentTour: this.CurrentTour,
                            groups: this.Groups,
                            createdAt: this.CreatedAt);
    }

    public TTournament ConvertToTeamTournament<TTournament>() where TTournament : ITeamTournament
    {
        return CreateTeamTournament<TTournament>(id: this.Id,
                                       name: this.Name,
                                       drawSystem: this.DrawSystem,
                                       coefficients: this.Coefficients,
                                       maxTour: this.MaxTour,
                                       currentTour: this.CurrentTour,
                                       createdAt: this.CreatedAt,
                                       groups: this.Groups,
                                       teams: (this as ITeamTournament)?.Teams
                                           ?? new List<Team>());
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

    private void SetDrawingProperties(DrawSystem drawSystem, IReadOnlyCollection<Coefficient> coefficients)
    {
        this.DrawSystem = drawSystem;
        this.UpdateCoefficients(coefficients);
    }

    public void UpdateCoefficients(IReadOnlyCollection<Coefficient> coefficients)
    {
        List<Coefficient> wrongCoefficients = coefficients.Except(GetPossibleCoefficients(this.DrawSystem)).ToList();
        if (wrongCoefficients.Any())
        {
            throw new DomainException($"Wrong coefficients for {this.DrawSystem} draw system: {
                string.Join(", ", wrongCoefficients)}");
        }

        this.Coefficients = coefficients;
    }

    public DrawResult DrawNewTour()
    {
        return this.DrawSystem switch
               {
                   DrawSystem.RoundRobin => this.DrawRoundRobin(),
                   DrawSystem.Swiss      => this.DrawSwiss(),
                   _ => throw new DomainOutOfRangeException(nameof(this.DrawSystem), this.DrawSystem,
                                                            "Cannot draw – unknown system"),
               };
    }

    private protected abstract DrawResult DrawSwiss();

    private protected abstract DrawResult DrawRoundRobin();
}

public interface ITeamTournament
{
    public List<Team> Teams { get; }
}
