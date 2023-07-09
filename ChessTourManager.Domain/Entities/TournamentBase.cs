using ChessTourManager.Domain.Exceptions;
using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase
{
    private protected TournamentBase(Id                               id,
                                     Name                             name,
                                     DrawSystem                       drawSystem,
                                     IReadOnlyCollection<Coefficient> coefficients,
                                     TourNumber                       toursAmount,
                                     DateOnly                         createdAt,
                                     TourNumber                       currentTour,
                                     List<Group>                      groups)
    {
        this.Id        = id;
        this.Name      = name;
        this.CreatedAt = createdAt;
        this.Groups    = groups;
        this.SetDrawingProperties(drawSystem, coefficients);
        this.SetTours(toursAmount, currentTour);
    }

    public Id Id { get; }

    public Name Name { get; set; }

    public DateOnly CreatedAt { get; }

    public DrawSystem DrawSystem { get; private set; }

    public IReadOnlyCollection<Coefficient> Coefficients { get; private set; }

    public Kind Kind { get; private protected set; }

    public TourNumber MaxTour { get; private set; }

    public TourNumber CurrentTour { get; private set; }

    public List<Group> Groups { get; }

    public IEnumerable<Player> Players
    {
        get => this.Groups.SelectMany(g => g.Players);
    }

    public static TTournament Create<TTournament>(Id                               id,
                                                  Name                             name,
                                                  DrawSystem                       drawSystem,
                                                  IReadOnlyCollection<Coefficient> coefficients,
                                                  Kind                             kind,
                                                  TourNumber                       toursAmount,
                                                  DateOnly                         createdAt,
                                                  TourNumber                       currentTour,
                                                  List<Group>                      groups)
        where TTournament : TournamentBase
    {
        TournamentBase tournament = kind switch
                                    {
                                        Kind.Single => new SingleTournament(id, name, drawSystem, coefficients,
                                                                            toursAmount, createdAt, currentTour, groups),
                                        Kind.Team => new TeamTournament(id, name, drawSystem, coefficients, toursAmount,
                                                                        createdAt, currentTour, groups),
                                        Kind.SingleTeam => new SingleTeamTournament(id, name, drawSystem, coefficients,
                                            toursAmount, createdAt, currentTour, groups),
                                        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
                                    };

        return (TTournament)tournament;
    }

    public static IEnumerable<Coefficient> GetPossibleCoefficients(DrawSystem drawSystem)
    {
        return drawSystem switch
               {
                   DrawSystem.RoundRobin => new List<Coefficient> { Coefficient.Berger, Coefficient.SimpleBerger },
                   DrawSystem.Swiss      => new List<Coefficient> { Coefficient.Buchholz, Coefficient.TotalBuchholz },
                   _                     => throw new ArgumentOutOfRangeException(nameof(drawSystem), drawSystem, null),
               };
    }

    public TTournament GetWithUpdatedKind<TTournament>(Kind kind) where TTournament : TournamentBase
    {
        return Create<TTournament>(this.Id,        this.Name, this.DrawSystem, this.Coefficients, kind, this.MaxTour,
                                   this.CreatedAt, this.CurrentTour, this.Groups);
    }

    public void SetTours(TourNumber maxTour, TourNumber currentTour)
    {
        if (maxTour < currentTour)
        {
            throw new DomainException("Max tour must be greater or equal to current tour");
        }

        this.MaxTour     = maxTour;
        this.CurrentTour = currentTour;
    }

    public void SetDrawingProperties(DrawSystem drawSystem, IReadOnlyCollection<Coefficient> coefficients)
    {
        this.DrawSystem = drawSystem;
        this.SetCoefficients(coefficients, drawSystem);
    }

    private void SetCoefficients(IReadOnlyCollection<Coefficient> coefficients, DrawSystem drawSystem)
    {
        List<Coefficient> wrongCoefficients = coefficients.Except(GetPossibleCoefficients(drawSystem)).ToList();
        if (wrongCoefficients.Any())
        {
            throw new DomainException(
                                        $"Wrong coefficients for {
                                            this.DrawSystem
                                        } draw system: {
                                            string.Join(", ", wrongCoefficients)
                                        }"
                                       );
        }

        this.Coefficients = coefficients;
    }

    public abstract DrawResult DrawNewTour();
}
