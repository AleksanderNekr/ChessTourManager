using ChessTourManager.Domain.ValueObjects;

namespace ChessTourManager.Domain.Entities;

public abstract class TournamentBase
{
    protected private TournamentBase(Name                             name,
                                     DrawSystem                       drawSystem,
                                     IReadOnlyCollection<Coefficient> coefficients,
                                     TourNumber                       toursAmount,
                                     DateOnly                         createdAt)
    {
        this.Id        = Guid.NewGuid();
        this.Name      = name;
        this.CreatedAt = createdAt;
        this.SetDrawingProperties(drawSystem, coefficients);
        this.SetMaxTour(toursAmount);
    }

    public static TTournament Create<TTournament>(Name                             name,
                                                  DrawSystem                       drawSystem,
                                                  IReadOnlyCollection<Coefficient> coefficients,
                                                  Kind                             kind,
                                                  TourNumber                       toursAmount,
                                                  DateOnly                         createdAt)
        where TTournament : TournamentBase
    {
        TournamentBase tournament = kind switch
                                    {
                                        Kind.Single => new SingleTournament(name, drawSystem, coefficients, toursAmount,
                                                                            createdAt),
                                        Kind.Team => new TeamTournament(name, drawSystem, coefficients, toursAmount,
                                                                        createdAt),
                                        Kind.SingleTeam => new SingleTeamTournament(name, drawSystem, coefficients,
                                            toursAmount, createdAt),
                                        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
                                    };

        return (TTournament)tournament;
    }

    public Guid Id { get; }

    public Name Name { get; set; }

    public DateOnly CreatedAt { get; }

    public DrawSystem DrawSystem { get; private set; }

    public IReadOnlyCollection<Coefficient> Coefficients { get; private set; } = default!;

    public Kind Kind { get; protected private set; }

    public TourNumber MaxTour { get; private set; }

    public TourNumber CurrentTour { get; private set; }

    public List<Group> Groups { get; private set; } = new();

    public IEnumerable<Player> Players
    {
        get => this.Groups.SelectMany(g => g.Players);
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
        return Create<TTournament>(this.Name, this.DrawSystem, this.Coefficients, kind, this.MaxTour, this.CreatedAt);
    }

    private void SetMaxTour(TourNumber toursAmount)
    {
        if (toursAmount < this.CurrentTour)
        {
            throw new ArgumentException("Tours amount must be greater or equal to current tour");
        }

        this.MaxTour = toursAmount;
    }

    public void SetDrawingProperties(DrawSystem drawSystem, IReadOnlyCollection<Coefficient> coefficients)
    {
        this.DrawSystem = drawSystem;
        this.SetCoefficients(coefficients, drawSystem);
    }

    public void SetCoefficients(IReadOnlyCollection<Coefficient> coefficients, DrawSystem drawSystem)
    {
        List<Coefficient> wrongCoefficients = coefficients.Except(GetPossibleCoefficients(drawSystem)).ToList();
        if (wrongCoefficients.Any())
        {
            throw new ArgumentException(
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
